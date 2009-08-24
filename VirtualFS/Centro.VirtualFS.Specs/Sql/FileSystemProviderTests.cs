using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Centro.OpenEntity.DataProviders;
using Centro.OpenEntity;
using Centro.VirtualFS.Sql;
using Centro.OpenEntity.Mapping;
using System.Collections;

namespace Centro.VirtualFS.Specs.Sql
{
    [TestFixture]
    public class FileSystemProviderTests
    {
        public const string SqlServerConnectionString = "data source=.\\SQLEXPRESS;Integrated Security=True;Initial Catalog=VirtualFS";
        public const string SqlServerProviderName = SqlDbClientTypeName.MSSQL;

        public static IDataProvider GetSqlServerDataProvider()
        {
            return DataProviderFactory.CreateNewProvider(SqlServerConnectionString, SqlServerProviderName, "dbo");
        }

        [TestFixtureSetUp]
        public void SetupRepository()
        {
            MappingTable.Clear();
            MappingTable.AddAssembly(typeof(SqlFileSystemProvider).Assembly);
        }

        [Test]
        public void Should_Create()
        {
            var provider = new SqlFileSystemProvider(GetSqlServerDataProvider());
        }

        [Test]
        public void Provider_Should_Get_Root_Directory()
        {
            var provider = new SqlFileSystemProvider(GetSqlServerDataProvider());

            var directory = provider.RootDirectory;

            Assert.IsNotNull(directory);
            Assert.AreEqual(VirtualPath.RootPath, directory.Path);
            Assert.IsEmpty(directory.Name);
        }

        [Test]
        public void Provider_Should_Get_Directory()
        {
            var provider = new SqlFileSystemProvider(GetSqlServerDataProvider());
            var directoryname = "Test";
            var virtualDirectoryPath = "/" + directoryname + "/";

            var directory = provider.GetDirectory(new VirtualPath(virtualDirectoryPath));

            Assert.IsNotNull(directory);
            Assert.AreEqual(virtualDirectoryPath, directory.Path.FullPath);
            Assert.AreEqual(directoryname, directory.Name);
        }

        [Test]
        public void Provider_Should_Get_Sub_Directory()
        {
            var provider = new SqlFileSystemProvider(GetSqlServerDataProvider());
            var directoryname = "Test";
            var subdirectoryname = "SubTest";
            var virtualDirectoryPath = "/" + directoryname + "/" + subdirectoryname + "/";

            var directory = provider.GetDirectory(new VirtualPath(virtualDirectoryPath));

            Assert.IsNotNull(directory);
            Assert.AreEqual(virtualDirectoryPath, directory.Path.FullPath);
            Assert.AreEqual(subdirectoryname, directory.Name);
        }

        [Test]
        public void Provider_Should_get_Sub_Directories()
        {
            var provider = new SqlFileSystemProvider(GetSqlServerDataProvider());
            var directoryname = "Test";
            var subdirectoryname = "SubTest";
            var virtualDirectoryPath = "/" + directoryname + "/";
            var virtualSubDirectoryPath = "/" + directoryname + "/" + subdirectoryname + "/";

            var directory = provider.GetDirectory(new VirtualPath(virtualDirectoryPath));
            var directories = provider.GetDirectories(directory);

            Assert.IsNotNull(directories);
            Assert.IsNotEmpty(directories as ICollection);
            Assert.AreEqual(1, directories.Count);
            Assert.AreEqual(subdirectoryname, directories[0].Name);
            Assert.AreEqual(virtualSubDirectoryPath, directories[0].Path.FullPath);
        }

    }
}
