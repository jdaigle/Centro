using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NHibernate;
using NHibernate.Linq;
using NorthwindTests.Model;
using System.Collections;

namespace NorthwindTests
{
    [TestFixture]
    public class TestConfiguration : TestBase
    {
        private ISession session;

        [SetUp]
        public void Setup()
        {
            session = GetNewSession();
        }

        [TearDown]
        public void TearDown()
        {
            session.Close();
        }

        [Test]
        public void Fetch_Some_Products_With_Linq()
        {
            var products = session.Linq<Product>().ToList();
            Assert.IsNotEmpty(products as ICollection);
            foreach (var product in products)
            {
                Assert.IsNotNullOrEmpty(product.Name);
                if (product.Category != null)
                    Assert.IsNotNullOrEmpty(product.Category.Name);
            }
            Assert.IsTrue(products.Any(x => x.Category != null));
        }

        [Test]
        public void Fetch_Some_Categories_With_Linq()
        {
            var categories = session.Linq<Category>().ToList();
            Assert.IsNotEmpty(categories as ICollection);
            foreach (var category in categories)
            {
                Assert.IsNotNullOrEmpty(category.Name);
                Assert.IsNotNullOrEmpty(category.Description);
            }
            Assert.IsTrue(categories.Any(x => x.Picture != null));
        }
    }
}
