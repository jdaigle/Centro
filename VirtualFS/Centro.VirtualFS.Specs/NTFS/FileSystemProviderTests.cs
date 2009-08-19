using System;
using System.IO;
using Centro.VirtualFS.NTFS;
using NUnit.Framework;

namespace Centro.VirtualFS.Specs.NTFS
{
    [TestFixture]
    public class FileSystemProviderTests
    {
        [Test]
        public void Provider_Should_Create()
        {
            new NTFSFileSystemProvider(Path.GetTempPath());
        }

        [Test]
        public void Provider_Should_Get_Root_Directory()
        {
            var provider = new NTFSFileSystemProvider(Path.GetTempPath());

            var directory = provider.RootDirectory;

            Assert.IsNotNull(directory);
            Assert.AreEqual(VirtualPath.RootPath, directory.Path);
            Assert.IsEmpty(directory.Name);
        }

        [Test]
        public void Provider_Should_Get_Directory()
        {
            var provider = new NTFSFileSystemProvider(Path.GetTempPath());
            var directoryname = Guid.NewGuid().ToString();
            var virtualDirectoryPath = "/" + directoryname + "/";

            try
            {
                Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), directoryname));
                var directory = provider.GetDirectory(new VirtualPath(virtualDirectoryPath));

                Assert.IsNotNull(directory);
                Assert.AreEqual(virtualDirectoryPath, directory.Path.FullPath);
                Assert.AreEqual(directoryname, directory.Name);
            }
            finally
            {
                Directory.Delete(Path.Combine(Path.GetTempPath(), directoryname));
            }
        }

        [Test]
        public void Provider_Should_Get_Sub_Directory()
        {
            var provider = new NTFSFileSystemProvider(Path.GetTempPath());
            var directoryname = Guid.NewGuid().ToString();
            var subdirectoryname = Guid.NewGuid().ToString();
            var virtualDirectoryPath = "/" + directoryname + "/" + subdirectoryname + "/";

            try
            {
                Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), directoryname + Path.DirectorySeparatorChar + subdirectoryname));
                var directory = provider.GetDirectory(new VirtualPath(virtualDirectoryPath));

                Assert.IsNotNull(directory);
                Assert.AreEqual(virtualDirectoryPath, directory.Path.FullPath);
                Assert.AreEqual(subdirectoryname, directory.Name);
            }
            finally
            {
                Directory.Delete(Path.Combine(Path.GetTempPath(), directoryname), true);
            }
        }
    }
}
