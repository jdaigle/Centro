using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Centro.OpenEntity.DataProviders;
using Centro.OpenEntity.Repository;
using Centro.OpenEntity.Query;
using Centro.Core.Extensions;

namespace Centro.VirtualFS.Sql
{
    public class SqlFileSystemProvider : IFileSystemProvider
    {
        private IRepository<SqlDirectory> directoryRepository;
        private IRepository<SqlFile> fileRepository;

        public SqlFileSystemProvider(IDataProvider dataProvider)
        {
            directoryRepository = new RepositoryBase<SqlDirectory>(dataProvider);
            fileRepository = new RepositoryBase<SqlFile>(dataProvider);
        }

        public SqlDirectory RootDirectory
        {
            get
            {
                return VerifyRootDirectoryExists();
            }
        }

        public SqlDirectory GetDirectory(VirtualPath path)
        {
            // TODO optimize this crap
            if (path.IsRoot)
                return RootDirectory;
            var pathParts = path.DirectoryParts;
            if (pathParts.Count == 0)
                throw new InvalidOperationException("Could not find any directory parts in the path.");
            var predicate = new PredicateExpression()
                    .Where<SqlDirectory>(x => x.Name).IsLike(pathParts[0])
                    .And<SqlDirectory>(x => x.IsDeleted).IsEqualTo(false)
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

        public SqlFile GetFile(VirtualPath path)
        {
            if (string.IsNullOrEmpty(path.FileName))
                return null;
            var directory = GetDirectory(path.DirectoryPart);
            if (directory == null)
                return null;
            var predicate = new PredicateExpression()
                    .Where<SqlFile>(x => x.Name).IsLike(path.FileName)
                    .And<SqlFile>(x => x.ParentDirectory).IsEqualTo(directory.Id)
                    .And<SqlFile>(x => x.IsDeleted).IsEqualTo(false);
            return fileRepository.FetchAll(predicate).FirstOrDefault();
        }

        public IList<SqlDirectory> GetDirectories(SqlDirectory directory)
        {
            var predicate = new PredicateExpression()
                    .Where<SqlDirectory>(x => x.ParentDirectory).IsEqualTo(directory.Id)
                    .And<SqlDirectory>(x => x.IsDeleted).IsEqualTo(false)
                    .And<SqlDirectory>(x => x.IsRoot).IsEqualTo(0);
            return directoryRepository.FetchAll(predicate);
        }

        public IList<SqlFile> GetFiles(SqlDirectory directory)
        {
            var predicate = new PredicateExpression()
                    .Where<SqlFile>(x => x.ParentDirectory).IsEqualTo(directory.Id)
                    .And<SqlFile>(x => x.IsDeleted).IsEqualTo(false);
            return fileRepository.FetchAll(predicate);
        }

        public SqlDirectory CreateDirectory(SqlDirectory parent, string name)
        {
            if (GetDirectories(parent).Any(x => x.Name.Matches(name)))
                throw new FileSystemException(string.Format("Directory with the name [{0}] already exists.", name));

            var directory = directoryRepository.Create();
            directory.ParentDirectory = parent;
            directory.IsDeleted = false;
            directory.IsRoot = false;
            directory.Name = name;

            if (!directoryRepository.Save(directory, true))
                throw new FileSystemException("Failed to save new directory.");
            return directory;
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

        #region IFileSystemProvider Members

        IDirectory IFileSystemProvider.RootDirectory
        {
            get { return RootDirectory; }
        }

        IDirectory IFileSystemProvider.GetDirectory(VirtualPath path)
        {
            return GetDirectory(path);
        }

        IList<IDirectory> IFileSystemProvider.GetDirectories(IDirectory directory)
        {
            if (directory is SqlDirectory)
                return GetDirectories((SqlDirectory)directory).Cast<IDirectory>().ToList();
            return new List<IDirectory>();
        }

        IList<IFile> IFileSystemProvider.GetFiles(IDirectory directory)
        {
            if (directory is SqlDirectory)
                return GetFiles((SqlDirectory)directory).Cast<IFile>().ToList();
            return new List<IFile>();
        }

        IFile IFileSystemProvider.GetFile(VirtualPath path)
        {
            return GetFile(path);
        }

        IDirectory IFileSystemProvider.CreateDirectory(IDirectory parent, string name)
        {
            if (parent is SqlDirectory)
                return CreateDirectory((SqlDirectory)parent, name);
            return null;
        }

        #endregion
    }
}
