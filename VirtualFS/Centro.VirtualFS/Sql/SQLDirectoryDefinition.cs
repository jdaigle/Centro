using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cridion.Framework.FileSystem.Data;

namespace Cridion.Framework.FileSystem
{
    public class SQLDirectoryDefinition : DirectoryDefinition, IDirectoryPrivilege
    {
        public long DirectoryID { get; private set; }

        public SQLDirectoryDefinition(Cridion.Framework.FileSystem.Data.Directory directory, String FullPath, SQLFileSystemProvider FileSystemProvider) : base (directory.Directoryname, FullPath, FileSystemProvider)
        {
            this.DirectoryID = directory.Directoryid;
        }

        public Boolean HasReadAccess(String Membername)
        {
            SQLFileAccess sqlfileaccess = new SQLFileAccess(this);
            AccessControlList acl = sqlfileaccess.GetParentInheritedACL(sqlfileaccess.GetDirectoryACL());
            if (acl.Aclreadall)
                return true;
            if (acl.Aclchangeall)
                return true;
            if (acl.Acladminall)
                return true;
            if (acl.ACLMemberExists(Membername))
            {
                ACLMember member = acl.GetACLMember(Membername);
                if (member.Aclread)
                    return true;
                if (member.Aclchange)
                    return true;
                if (member.Acladmin)
                    return true;
            }
            return false;
        }

        public Boolean HasChangeAccess(String Membername)
        {
            SQLFileAccess sqlfileaccess = new SQLFileAccess(this);
            AccessControlList acl = sqlfileaccess.GetParentInheritedACL(sqlfileaccess.GetDirectoryACL());
            if (acl.Aclchangeall)
                return true;
            if (acl.Acladminall)
                return true;
            if (acl.ACLMemberExists(Membername))
            {
                ACLMember member = acl.GetACLMember(Membername);
                if (member.Aclchange)
                    return true;
                if (member.Acladmin)
                    return true;
            }
            return false;
        }

        public Boolean HasAdminAccess(String Membername)
        {
            SQLFileAccess sqlfileaccess = new SQLFileAccess(this);
            AccessControlList acl = sqlfileaccess.GetParentInheritedACL(sqlfileaccess.GetDirectoryACL());
            if (acl.Acladminall)
                return true;
            if (acl.ACLMemberExists(Membername))
            {
                ACLMember member = acl.GetACLMember(Membername);
                if (member.Acladmin)
                    return true;
            }
            return false;
        }
    }
}
