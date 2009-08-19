using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.VirtualFS
{
    public interface IFile
    {
        string Name { get; }
        VirtualPath Path { get; }
        long Size { get; }
        DateTime CreationTime { get; }
        DateTime LastWriteTime { get; }
    }
}
