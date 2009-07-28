using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenEntity.CodeDom;
using OpenEntity.Tests.Mock.Northwind;

namespace OpenEntity.Tests.CodeDom
{
    [TestClass]
    public class ProxyFactoryTests
    {
        public ProxyFactoryTests()
        {
        }
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void SetupMappings(TestContext testContext)
        {
            MappingConfig.AddAssembly(typeof(TestEnvironment).Assembly);
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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
