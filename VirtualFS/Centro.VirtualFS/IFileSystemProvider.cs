using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.VirtualFS
{
    public interface IFileSystemProvider
    {
        IDirectory RootDirectory { get; }
        IDirectory GetDirectory(VirtualPath path);
        IFile GetFile(VirtualPath path);
        IList<IDirectory> GetDirectories(IDirectory directory);
        IList<IFile> GetFiles(IDirectory directory);

        // Directory manipulation
        IDirectory CreateDirectory(IDirectory parent, string name);      
        void DeleteDirectory(IDirectory directory);
        void DeleteDirectory(VirtualPath path);

        // File manipulation
        IFile CreateFile(IDirectory directory, string name, byte[] data);
        IFile RenameFile(IFile file, string name);

        bool SupportsAccessControl { get; }
        bool SupportsFileVersioning { get; }
    }
}
