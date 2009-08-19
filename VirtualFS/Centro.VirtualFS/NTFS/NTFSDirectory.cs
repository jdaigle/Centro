using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Centro.VirtualFS.NTFS
{
    internal class NTFSDirectory : IDirectory
    {
        private NTFSFileSystemProvider fileSystemProvider;

        public NTFSDirectory(NTFSFileSystemProvider fileSystemProvider, string name, VirtualPath path)
        {
            this.fileSystemProvider = fileSystemProvider;
            Name = name;
            if (path.IsRoot)
                Name = string.Empty;
            Path = path;
        }

        public string Name { get; private set; }

        public VirtualPath Path { get; private set; }

        public IList<IDirectory> SubDirectories
        {
            get { return fileSystemProvider.GetDirectories(this); }
        }

        public IList<IFile> Files
        {
            get { return fileSystemProvider.GetFiles(this); }
        }

        public IDirectory ParentDirectory
        {
            get
            {
                if (Path.IsRoot)
                    return null;
                var removedLastChar = Path.Directory.Remove(Path.Directory.Length - 1);

                var parentPath = new VirtualPath(removedLastChar.Substring(0, removedLastChar.LastIndexOf(VirtualPath.DirectorySeparatorChar) + 1));
                return fileSystemProvider.GetDirectory(parentPath);
            }
        }
    }
}
