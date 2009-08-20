using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Centro.OpenEntity.Model;

namespace Centro.VirtualFS.Sql
{
    public class SqlDirectory : IDomainObject, IDirectory, IAccessControlledDirectory
    {
        public SqlDirectory()
        {
            SubDirectories = new List<SqlDirectory>();
        }

        public virtual int Id { get; private set; }
        public virtual bool IsRoot { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual string Name { get; set; }       
        public virtual SqlDirectory ParentDirectory { get; set; }
        public virtual IList<SqlDirectory> SubDirectories { get; private set; }
        public virtual IList<SqlFile> Files { get; private set; }

        public VirtualPath Path
        {
            get
            {
                if (IsRoot)
                    return VirtualPath.RootPath;
                return new VirtualPath(ParentDirectory.Path + Name + VirtualPath.DirectorySeparatorChar);
            }
        }

        public bool HasReadAccess(string username)
        {
            throw new NotImplementedException();
        }

        public bool HasChangeAccess(string username)
        {
            throw new NotImplementedException();
        }

        public bool HasAdminAccess(string username)
        {
            throw new NotImplementedException();
        }

        IDirectory IDirectory.ParentDirectory
        {
            get { return ParentDirectory; }
        }

        IList<IDirectory> IDirectory.SubDirectories
        {
            get { return SubDirectories.Cast<IDirectory>().ToList(); }
        }

        IList<IFile> IDirectory.Files
        {
            get { return Files.Cast<IFile>().ToList(); }
        }
    }
}
