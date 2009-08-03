using NUnit.Framework;
using OpenEntity.Entities;
using OpenEntity.Tests.Mock.Northwind;
using OpenEntity.DataProviders;

namespace OpenEntity.Specs.Repository
{
    public abstract class Fetch<TEntity> : RepositoryTestBase<TEntity>
    {
        [Test]
        public void ShouldReturnEntity()
        {
            var entity = Repository.Fetch(null);
            Assert.IsNotNull(entity);
            Assert.IsInstanceOf(typeof(IEntity), entity);
            Assert.IsInstanceOf(typeof(TEntity), entity);
        }

        [Test]
        public void EntityShouldBeFetched()
        {
            var entity = Repository.Fetch(null) as IEntity;
            Assert.AreEqual(entity.Fields.State, EntityState.Fetched);
        }

        [Test]
        public void EntityShouldNotBeNull()
        {
            var entity = Repository.Fetch(null) as IEntity;
            Assert.IsFalse(entity.IsNew);
        }

        [Test]
        public void EntityShouldNotBeDirty()
        {
            var entity = Repository.Fetch(null) as IEntity;
            Assert.IsFalse(entity.IsDirty);
        }
    }

    [TestFixture]
    public class SqlFetch : Fetch<Product>
    {
        protected override IDataProvider GetDataProvider()
        {
            return TestEnvironment.GetSqlServerDataProvider();
        }
    }
}
