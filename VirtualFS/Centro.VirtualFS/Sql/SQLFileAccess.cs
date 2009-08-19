using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cridion.Framework.FileSystem.Data;
using Cridion.Framework.Configuration;

namespace Cridion.Framework.FileSystem
{
    public class SQLFileAccess
    {

        protected FileExplorerDBContext db;

        private SQLDirectoryDefinition _directory;
        public SQLDirectoryDefinition Directory
        {
            get { return _directory; }
        }


        public SQLFileAccess(SQLDirectoryDefinition Directory)
        {   
            _directory = Directory;
            db = new FileExplorerDBContext(ConnectionStrings.GetConnectionString("FileExplorer"), System.Data.Linq.Mapping.XmlMappingSource.FromUrl(Cridion.Framework.Web.Utility.GetPhysicalAppPath() + LinqMappingConfiguration.Default["FileExplorer"]));
        }

        public AccessControlList GetDirectoryACL()
        {
            var acl = from a in db.AccessControlLists
                      where a.Directoryid == _directory.DirectoryID
                      select a;
            if (acl.Count() < 1)
            {
                CreateDefaultACL();
                acl = from a in db.AccessControlLists
                      where a.Directoryid == _directory.DirectoryID
                      select a;
            }
            return acl.First();
        }

        public void SetUniversalRead(Boolean Value)
        {
            AccessControlList acl = GetDirectoryACL();
            acl.Aclreadall = Value;
            db.SubmitChanges();
        }

        public void SetUniversalChange(Boolean Value)
        {
            AccessControlList acl = GetDirectoryACL();
            acl.Aclchangeall = Value;
            db.SubmitChanges();
        }

        public void SetInherit(Boolean Value)
        {
            AccessControlList acl = GetDirectoryACL();
            acl.Inherit = Value;
            db.SubmitChanges();
            if (!Value && !acl.IsRootDirectory)
                CopyFromParent();
        }
        
        public AccessControlList GetParentACL()
        {
            return GetParentACL(GetDirectoryACL());
        }

        public AccessControlList GetParentACL(AccessControlList ChildACL)
        {
            if (ChildACL.IsRootDirectory)
                throw new Exception("This is root, there is no parent directory");
            var acl = from a in db.AccessControlLists
                      where a.Directoryid == (from d in db.Directories
                                              where d.Directoryid == ChildACL.Directoryid
                                              select d.Parentdirectoryid).First()
                      select a;
            return acl.First();
        }

        public AccessControlList GetParentInheritedACL(AccessControlList ChildACL)
        {
            if (ChildACL.IsRootDirectory || !ChildACL.Inherit)
                return ChildACL;
            return GetParentInheritedACL(GetParentACL(ChildACL));
        }

        public void CopyFromParent()
        {
            AccessControlList thisACL = GetDirectoryACL();
            AccessControlList parentACL = GetParentInheritedACL(GetParentACL());            
            thisACL.Aclchangeall = parentACL.Aclchangeall;
            thisACL.Acladminall = parentACL.Acladminall;
            thisACL.Aclreadall = parentACL.Aclreadall;
            db.SubmitChanges();
            foreach (ACLMember member in parentACL.ACLMembers)
            {
                AddACLMember(member.Membername, member.Aclread, member.Aclchange, member.Acladmin);
            }            
        }

        public void MakeAdminSubDirs(String MemberName)
        {           
            foreach (SQLDirectoryDefinition directory in _directory.Directories)
            {
                SQLFileAccess subacl = new SQLFileAccess(directory);
                subacl.AddACLMember(MemberName, false, false, true);
            }
        }

        private void CreateDefaultACL()
        {
            AccessControlList acl = new AccessControlList();
            acl.Directoryid = _directory.DirectoryID;
            acl.Aclreadall = true;
            acl.Aclchangeall = true;
            acl.Acladminall = false;
            acl.Inherit = true;
            db.AccessControlLists.InsertOnSubmit(acl);
            db.SubmitChanges();
        }        

        public void AddACLMember(String MemberName, Boolean read, Boolean change, Boolean admin)
        {
            AccessControlList acl = GetDirectoryACL();

            if (acl.ACLMemberExists(MemberName))
            {
                ModifyACLMember(MemberName, read, change, admin);
                return;
            }

            ACLMember member = new ACLMember();
            member.Membername = MemberName;
            member.Acladmin = admin;
            member.Aclchange = change;
            member.Aclread = read;

            
            acl.ACLMembers.Add(member);

            db.SubmitChanges();
        }

        public void ModifyACLMember(String MemberName, Boolean read, Boolean change, Boolean admin)
        {
            AccessControlList acl = GetDirectoryACL();

            if (!acl.ACLMemberExists(MemberName))
            {
                AddACLMember(MemberName, read, change, admin);
                return;
            }

            ACLMember member = acl.GetACLMember(MemberName);
            member.Membername = MemberName;
            member.Acladmin = admin;
            member.Aclchange = change;
            member.Aclread = read;

            db.SubmitChanges();
        }

        public void RemoveACLMemeber(String MemberName)
        {
            AccessControlList acl = GetDirectoryACL();

            if (!acl.ACLMemberExists(MemberName))
                return;

            ACLMember member = acl.GetACLMember(MemberName);
            db.ACLMembers.DeleteOnSubmit(member);

            db.SubmitChanges();

        }

    }
}
