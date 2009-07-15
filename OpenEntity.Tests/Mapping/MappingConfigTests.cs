using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenEntity.Mapping;
using OpenEntity.Tests.Mock.Northwind;

namespace OpenEntity.Tests.Mapping
{
    [TestClass]
    public class MappingConfigTests
    {
        public MappingConfigTests()
        {
        }
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestInitialize]
        public void ClearMappings()
        {
            MappingConfig.Clear();
        }

        [TestMethod]        
        public void DefaultMappingConfigShouldNotFindAnyClassMappings()
        {
            foreach (var type in TestEnvironment.EntityTypes)
            {
                Assert.IsNull(MappingConfig.FindClassMapping(type));
            }
        }

        [TestMethod]
        public void AddingAssemblyShouldFindAllClassMappings()
        {
            MappingConfig.AddAssembly(typeof(TestEnvironment).Assembly);
            foreach (var type in TestEnvironment.EntityTypes)
            {
                Assert.IsNotNull(MappingConfig.FindClassMapping(type));
            }
        }

        [TestMethod]
        public void ClearShouldClearSearchedAssemblies()
        {
            MappingConfig.AddAssembly(typeof(TestEnvironment).Assembly);
            foreach (var type in TestEnvironment.EntityTypes)
            {
                Assert.IsNotNull(MappingConfig.FindClassMapping(type));
            }
            MappingConfig.Clear();
            foreach (var type in TestEnvironment.EntityTypes)
            {
                Assert.IsNull(MappingConfig.FindClassMapping(type));
            }
        }
    }
}
