using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.VirtualFS
{
    public interface IDirectory
    {
        string Name { get; }
        VirtualPath Path { get; }
        IDirectory ParentDirectory { get; }
        IList<IDirectory> SubDirectories { get; }
        IList<IFile> Files { get; }
    }
}
