using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Mapping;
using NUnit.Framework;
using OpenEntity.CodeDom;
using OpenEntity.Specs.Mock.Northwind;

namespace OpenEntity.Specs.CodeDom
{
    [TestFixture]
    public class ProxyFactoryTests
    {
        [TestFixtureSetUp]
        public static void SetupMappings()
        {
            MappingConfiguration.AddAssembly(typeof(TestEnvironment).Assembly);
        }

        [Test]
        public void ShouldThrowExceptionForUnmappedClass()
        {
            try
            {
                ProxyFactory.GetProxyClass(typeof(object));
            }
            catch (NotSupportedException)
            {
                // Expected
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void ShouldGetProxyTypeForMappedClassWithNullableTypes()
        {
            Assert.IsNotNull(ProxyFactory.GetProxyClass(typeof(Customer)));
        }

        [Test]
        public void ShouldGetProxyTypeForMappedClass()
        {
            Assert.IsNotNull(ProxyFactory.GetProxyClass(typeof(Category)));
            Assert.IsNotNull(ProxyFactory.GetProxyClass(typeof(Customer)));
            Assert.IsNotNull(ProxyFactory.GetProxyClass(typeof(Employee)));
            Assert.IsNotNull(ProxyFactory.GetProxyClass(typeof(Order)));
            Assert.IsNotNull(ProxyFactory.GetProxyClass(typeof(OrderInfo)));
            Assert.IsNotNull(ProxyFactory.GetProxyClass(typeof(Product)));
            Assert.IsNotNull(ProxyFactory.GetProxyClass(typeof(Shipper)));
            Assert.IsNotNull(ProxyFactory.GetProxyClass(typeof(Supplier)));
        }

        [Test]
        public void ShouldGetCachedProxyTypeForMappedClass()
        {
            var proxyType = ProxyFactory.GetProxyClass(typeof(Category));
            Assert.AreEqual(proxyType, ProxyFactory.GetProxyClass(typeof(Category)));
            proxyType = ProxyFactory.GetProxyClass(typeof(Customer));
            Assert.AreEqual(proxyType, ProxyFactory.GetProxyClass(typeof(Customer)));
            proxyType = ProxyFactory.GetProxyClass(typeof(Employee));
            Assert.AreEqual(proxyType, ProxyFactory.GetProxyClass(typeof(Employee)));
            proxyType = ProxyFactory.GetProxyClass(typeof(Order));
            Assert.AreEqual(proxyType, ProxyFactory.GetProxyClass(typeof(Order)));
            proxyType = ProxyFactory.GetProxyClass(typeof(OrderInfo));
            Assert.AreEqual(proxyType, ProxyFactory.GetProxyClass(typeof(OrderInfo)));
            proxyType = ProxyFactory.GetProxyClass(typeof(Product));
            Assert.AreEqual(proxyType, ProxyFactory.GetProxyClass(typeof(Product)));
            proxyType = ProxyFactory.GetProxyClass(typeof(Shipper));
            Assert.AreEqual(proxyType, ProxyFactory.GetProxyClass(typeof(Shipper)));
            proxyType = ProxyFactory.GetProxyClass(typeof(Supplier));
            Assert.AreEqual(proxyType, ProxyFactory.GetProxyClass(typeof(Supplier)));
        }

    }
}
