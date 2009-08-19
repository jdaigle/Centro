using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.VirtualFS
{
    public interface IVersion
    {
        IVersionedFile File { get; }

        int VersionNumber { get; }

        DateTime Modified { get; }
        
        Boolean IsLatestVersion { get; }

        long Size { get; }
    }
}
