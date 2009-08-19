using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.VirtualFS
{
    public interface IVersionedFile : IFile
    {
        IVersion LastestVersion { get; }

        IList<IVersion> Versions { get; }

        IVersion GetVersion(int versionNumber);
    }
}