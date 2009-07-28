using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenEntity.Mapping;
using OpenEntity.Tests.Mock.Northwind;
using OpenEntity.CodeDom;
using OpenEntity.Entities;

namespace OpenEntity.Tests.CodeDom
{
    [TestClass]
    public class ProxyGeneratorTests
    {
        public ProxyGeneratorTests()
        {
        }
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void SetupMappings(TestContext testContext)
        {
            MappingConfig.AddAssembly(typeof(TestEnvironment).Assembly);
        }

        [TestMethod]
        public void ShouldReturnProxyObject()
        {
            foreach (var type in TestEnvironment.EntityTypes)
            {
                var mapping = MappingConfig.FindClassMapping(type);
                var gen = new ProxyGenerator(mapping);
                var proxy = gen.Build();
                Assert.IsNotNull(proxy);
                Assert.IsTrue(type.IsAssignableFrom(proxy));
                Assert.IsTrue(typeof(IProxyEntity).IsAssignableFrom(proxy));
                Assert.IsTrue(typeof(IEntity).IsAssignableFrom(proxy));
            }
        }
    }
}
