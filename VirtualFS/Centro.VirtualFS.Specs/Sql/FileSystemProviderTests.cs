using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Centro.OpenEntity.DataProviders;
using Centro.OpenEntity;
using Centro.VirtualFS.Sql;
using Centro.OpenEntity.Mapping;

namespace Centro.VirtualFS.Specs.Sql
{
    [TestFixture]
    public class FileSystemProviderTests
    {
        public const string SqlServerConnectionString = "data source=.;Integrated Security=True;Initial Catalog=FileExplorerDatabase";
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
    }
}
