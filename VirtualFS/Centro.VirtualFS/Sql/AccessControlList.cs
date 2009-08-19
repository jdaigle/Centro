using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cridion.Framework.FileSystem.Data
{
    public partial class AccessControlList
    {

        public Boolean IsRootDirectory
        {
            get { return this.Directory.Rootdirectory; }
        }

        public Boolean ACLMemberExists(String Member)
        {
            return (from a in this.ACLMembers
                    where a.Membername.Equals(Member)
                    select a).Count() > 0;
        }

        public ACLMember GetACLMember(String Member)
        {
            return (from a in this.ACLMembers
                    where a.Membername.Equals(Member)
                    select a).First();
        }

    }
}
