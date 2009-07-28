using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenEntity.DataProviders;
using System.Configuration;

namespace OpenEntity.Tests.DataProviders
{
    [TestClass]
    public class DataProviderFactoryTests
    {
        public DataProviderFactoryTests()
        {
        }
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void SetupNamedConnectionString(TestContext context)
        {
            var settings = new ConnectionStringSettings("default", TestEnvironment.ConnectionString, TestEnvironment.ProviderName);
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);            
            configuration.ConnectionStrings.ConnectionStrings.Add(settings);
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("ConnectionStrings");
        }

        [ClassCleanup]
        public static void RemoveNamedConnectionString()
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.ConnectionStrings.ConnectionStrings.Remove("default");
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("ConnectionStrings");
        }

        [TestMethod]
        public void CreateWithConnectionStringShouldReturnCorrectProvider()
        {
            var provider = DataProviderFactory.CreateNewProvider(TestEnvironment.ConnectionString, TestEnvironment.ProviderName, null);
            Assert.IsNotNull(provider);
            Assert.IsTrue(provider is SqlServerDataProvider);
        }

        [TestMethod]
        public void CreateWithConnectionStringNameShouldReturnCorrectProvider()
        {            
            var provider = DataProviderFactory.CreateNewProvider("default");
            Assert.IsNotNull(provider);
            Assert.IsTrue(provider is SqlServerDataProvider);
        }
    }
}
