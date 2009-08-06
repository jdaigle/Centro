using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenEntity.Mapping;
using OpenEntity.Specs.Mock.Northwind;
using OpenEntity.CodeDom;
using OpenEntity.Entities;

namespace OpenEntity.Specs.CodeDom
{
    [TestFixture]
    public class ProxyGeneratorTests
    {
        [TestFixtureSetUp]
        public static void SetupMappings()
        {
            MappingConfiguration.AddAssembly(typeof(TestEnvironment).Assembly);
        }

        [Test]
        public void ShouldReturnProxyObject()
        {
            foreach (var type in TestEnvironment.EntityTypes)
            {
                var mapping = MappingConfiguration.FindClassConfiguration(type);
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
