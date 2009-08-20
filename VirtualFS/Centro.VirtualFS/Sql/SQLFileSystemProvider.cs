using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Centro.OpenEntity.DataProviders;
using Centro.OpenEntity.Repository;
using Centro.OpenEntity.Query;
using Centro.OpenEntity.Extensions;

namespace Centro.VirtualFS.Sql
{
    public class SqlFileSystemProvider : IFileSystemProvider
    {
        private IRepository<SqlDirectory> directoryRepository;

        public SqlFileSystemProvider(IDataProvider dataProvider)
        {
            directoryRepository = new RepositoryBase<SqlDirectory>(dataProvider);
        }

        public IDirectory RootDirectory
        {
            get
            {
                return VerifyRootDirectoryExists();
            }
        }

        public IDirectory GetDirectory(VirtualPath path)
        {
            // TODO optimize this crap
            if (path.IsRoot)
                return RootDirectory;
            var pathParts = path.DirectoryParts;
            if (pathParts.Count == 0)
                throw new InvalidOperationException("Could not find any directory parts in the path.");
            var predicate = new PredicateExpression()
                    .Where<SqlDirectory>(x => x.Name).IsLike(pathParts[0])
                    .And<SqlDirectory>(x => x.IsRoot).IsEqualTo(0);
            var matchingDirectories = directoryRepository.FetchAll(predicate);
            if (matchingDirectories.Count == 0)
                return null;
            foreach (var directory in matchingDirectories)
            {
                var currentDirectory = directory.ParentDirectory;
                for (int i = 1; i < pathParts.Count + 1; i++)
                {
                    if (i == pathParts.Count && currentDirectory.IsRoot)
                        return directory;
                    var part = pathParts[i];
                    if (currentDirectory.Name.Matches(part))
                    {
                        currentDirectory = currentDirectory.ParentDirectory;
                        continue;
                    }
                    else
                        break;
                }
            }
            return null;
        }

        public IFile GetFile(VirtualPath path)
        {
            throw new NotImplementedException();
        }

        public IList<IDirectory> GetDirectories(IDirectory directory)
        {
            throw new NotImplementedException();
        }

        public IList<IFile> GetFiles(IDirectory directory)
        {
            throw new NotImplementedException();
        }

        public IDirectory CreateDirectory(IDirectory parent, string name)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(IDirectory directory)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(VirtualPath path)
        {
            throw new NotImplementedException();
        }

        public IFile CreateFile(IDirectory directory, string name, byte[] data)
        {
            throw new NotImplementedException();
        }

        public IFile RenameFile(IFile file, string name)
        {
            throw new NotImplementedException();
        }

        public bool SupportsAccessControl
        {
            get { throw new NotImplementedException(); }
        }

        public bool SupportsFileVersioning
        {
            get { throw new NotImplementedException(); }
        }

        private SqlDirectory VerifyRootDirectoryExists()
        {
            var predicate = new PredicateExpression().Where<SqlDirectory>(x => x.IsRoot).IsEqualTo(true);
            var directory = directoryRepository.Fetch(predicate);
            if (directory == null)
            {
                directory = directoryRepository.Create();
                directory.IsRoot = true;
                directory.Name = string.Empty;
                directory.IsDeleted = false;
                if (!directoryRepository.Save(directory, true))
                    throw new FileSystemException("Failed to create root virtual filesystem directory in the SQL database.");
            }
            return directory;
        }
    }
}
