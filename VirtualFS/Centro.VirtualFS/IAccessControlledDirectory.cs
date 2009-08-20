using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.VirtualFS
{
    public interface IAccessControlledDirectory : IDirectory
    {
        bool HasReadAccess(string username);

        bool HasChangeAccess(string username);

        bool HasAdminAccess(string username);
    }
}