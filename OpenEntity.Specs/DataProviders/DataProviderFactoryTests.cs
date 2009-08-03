using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenEntity.DataProviders;
using System.Configuration;

namespace OpenEntity.Specs.DataProviders
{
    [TestFixture]
    public class DataProviderFactoryTests
    {
        [TestFixtureSetUp]
        public static void SetupNamedConnectionString()
        {
            var settings1 = new ConnectionStringSettings("default", TestEnvironment.SqlServerConnectionString, TestEnvironment.SqlServerProviderName);
            var settings2 = new ConnectionStringSettings("noprovidername", TestEnvironment.SqlServerConnectionString, string.Empty);
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.ConnectionStrings.ConnectionStrings.Add(settings1);
            configuration.ConnectionStrings.ConnectionStrings.Add(settings2);
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("ConnectionStrings");
        }

        [TestFixtureTearDown]
        public static void RemoveNamedConnectionString()
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.ConnectionStrings.ConnectionStrings.Remove("default");
            configuration.ConnectionStrings.ConnectionStrings.Remove("noprovidername");
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("ConnectionStrings");
        }

        [Test]
        public void CreateWithConnectionStringShouldReturnCorrectProvider()
        {
            var provider = DataProviderFactory.CreateNewProvider(TestEnvironment.SqlServerConnectionString, TestEnvironment.SqlServerProviderName, null);
            Assert.IsNotNull(provider);
            Assert.IsTrue(provider is SqlServerDataProvider);
        }

        [Test]
        public void CreateWithConnectionStringNameShouldReturnCorrectProvider()
        {
            var provider = DataProviderFactory.CreateNewProvider("default");
            Assert.IsNotNull(provider);
            Assert.IsTrue(provider is SqlServerDataProvider);
        }
    }
}
