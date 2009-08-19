using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Centro.VirtualFS.NTFS;
using System.IO;

namespace Centro.VirtualFS.Specs.NTFS
{
    [TestFixture]
    public class DirectoryTests
    {

        [Test]
        public void ParentDirectory_Should_Get_RootDirectory()
        {
            var provider = new NTFSFileSystemProvider(Path.GetTempPath());
            var directoryname = Guid.NewGuid().ToString();
            var virtualDirectoryPath = "/" + directoryname + "/";

            try
            {
                Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), directoryname));
                var directory = provider.GetDirectory(new VirtualPath(virtualDirectoryPath));
                var parentDirectory = directory.ParentDirectory;

                Assert.IsNotNull(parentDirectory);
                Assert.AreEqual(VirtualPath.RootPath, parentDirectory.Path);
                Assert.IsEmpty(parentDirectory.Name);
            }
            finally
            {
                Directory.Delete(Path.Combine(Path.GetTempPath(), directoryname));
            }
        }

        [Test]
        public void ParentDirectory_Should_Get_Parent_Directory()
        {
            var provider = new NTFSFileSystemProvider(Path.GetTempPath());
            var directoryname = Guid.NewGuid().ToString();
            var subdirectoryname = Guid.NewGuid().ToString();
            var virtualDirectoryPath = "/" + directoryname + "/" + subdirectoryname + "/";

            try
            {
                Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), directoryname + Path.DirectorySeparatorChar + subdirectoryname));
                var directory = provider.GetDirectory(new VirtualPath(virtualDirectoryPath));
                var parentDirectory = directory.ParentDirectory;

                Assert.IsNotNull(parentDirectory);
                Assert.AreEqual(new VirtualPath("/" + directoryname + "/"), parentDirectory.Path);
                Assert.AreEqual(directoryname, parentDirectory.Name);
            }
            finally
            {
                Directory.Delete(Path.Combine(Path.GetTempPath(), directoryname), true);
            }
        }
    }
}
