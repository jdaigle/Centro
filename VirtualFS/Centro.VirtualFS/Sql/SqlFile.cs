using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Centro.OpenEntity.Model;

namespace Centro.VirtualFS.Sql
{
    public class SqlFile : IDomainObject, IFile
    {
        public virtual int Id { get; private set; }
        public virtual string Name { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual SqlDirectory ParentDirectory { get; set; }
        public virtual bool IsDeleted { get; set; }

        public long Size
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime LastWriteTime
        {
            get { throw new NotImplementedException(); }
        }

        public VirtualPath Path
        {
            get { throw new NotImplementedException(); }
        }
    }
}
