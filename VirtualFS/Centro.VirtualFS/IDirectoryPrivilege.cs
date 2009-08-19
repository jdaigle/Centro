using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.VirtualFS
{
    /// <summary>
    /// Interface for handling privilege enabled directories
    /// </summary>
    public interface IDirectoryPrivilege
    {
        /// <summary>
        /// This member has read access on this directory
        /// </summary>
        /// <param name="Membername"></param>
        /// <returns>This member has read access on this directory</returns>
        Boolean HasReadAccess(String Membername);

        /// <summary>
        /// This member has change access on this directory
        /// </summary>
        /// <param name="Membername"></param>
        /// <returns>This member has change access on this directory</returns>
        Boolean HasChangeAccess(String Membername);


        /// <summary>
        /// This member has admin access on this directory
        /// </summary>
        /// <param name="Membername"></param>
        /// <returns>This member has admin access on this directory</returns>
        Boolean HasAdminAccess(String Membername);

    }
}