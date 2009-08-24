using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Centro.OpenEntity;
using Centro.OpenEntity.DataProviders;
using Centro.OpenEntity.Mapping;
using Centro.VirtualFS.Sql;
using System.Collections;

namespace Centro.VirtualFS.Specs.Sql
{
    [TestFixture]
    public class DirectoryTests
    {
        [TestFixtureSetUp]
        public void SetupRepository()
        {
            MappingTable.Clear();
            MappingTable.AddAssembly(typeof(SqlFileSystemProvider).Assembly);
        }

        [Test]
        public void Root_Directory_Should_Have_Directories()
        {
            var provider = new SqlFileSystemProvider(FileSystemProviderTests.GetSqlServerDataProvider());
            var root = provider.RootDirectory;

            var directories = root.SubDirectories;

            Assert.IsNotNull(directories);
            Assert.IsNotEmpty(directories as ICollection);
            foreach (var directory in directories)
            {
                Assert.IsNotNullOrEmpty(directory.Name);
            }
        }

        [Test]
        public void Directory_Subdirectories_Should_Have_Subdirectories()
        {
            var provider = new SqlFileSystemProvider(FileSystemProviderTests.GetSqlServerDataProvider());
            var root = provider.RootDirectory;

            var directories = root.SubDirectories;

            foreach (var directory in directories)
            {
                var subdirectories = directory.SubDirectories;
                Assert.IsNotNull(subdirectories);
                foreach (var subdirectory in subdirectories)
                {
                    Assert.IsNotNullOrEmpty(subdirectory.Name);
                }
            }
        }
    }
}
