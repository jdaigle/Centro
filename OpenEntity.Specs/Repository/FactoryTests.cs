using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenEntity.Repository;
using OpenEntity.Specs.Mock.Northwind;

namespace OpenEntity.Specs.Repository
{
    [TestFixture]
    public class FactoryTests
    {
        [Test]
        public void Factory_Should_Return_Repository()
        {
            var factory = new RepositoryFactory(TestEnvironment.GetSqlServerDataProvider());

            var repository = factory.GetRepository<Product>();

            Assert.IsNotNull(repository);
        }

        [Test]
        public void Factory_Should_Return_Cached_Repository()
        {
            var factory = new RepositoryFactory(TestEnvironment.GetSqlServerDataProvider());

            var repository = factory.GetRepository<Product>();
            var cachedRepository = factory.GetRepository<Product>();

            Assert.AreEqual(repository, cachedRepository);
        }

        [Test]
        public void Factory_Should_Be_Cached()
        {
            var dataProvider = TestEnvironment.GetSqlServerDataProvider();
            var factory = new RepositoryFactory(dataProvider);
            var cachedFactory = RepositoryFactory.GetRepositoryFactoryFor(dataProvider);

            Assert.AreEqual(factory, cachedFactory);
        }
    }
}
