using NUnit.Framework;
using OpenEntity.Entities;
using OpenEntity.Specs.Mock.Northwind;
using OpenEntity.DataProviders;
using OpenEntity.Model;

namespace OpenEntity.Specs.Repository
{
    public abstract class Fetch<TModelType> : RepositoryTestBase<TModelType> where TModelType : IDomainObject
    {
        [Test]
        public void ShouldReturnEntity()
        {
            var entity = Repository.Fetch(null);
            Assert.IsNotNull(entity);
            Assert.IsInstanceOf(typeof(IEntity), entity);
            Assert.IsInstanceOf(typeof(TModelType), entity);
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

        [Test]
        public void EntityShouldHaveFieldValues()
        {
            var entity = Repository.Fetch(null) as IEntity;
            foreach (var field in entity.Fields)
            {
                if (!field.IsNull)
                    Assert.IsNotNull(field.CurrentValue);
            }
        }
    }

    [TestFixture]
    public class SqlFetch1 : Fetch<Product>
    {
        protected override IDataProvider GetDataProvider()
        {
            return TestEnvironment.GetSqlServerDataProvider();
        }
    }

    [TestFixture]
    public class SqlFetch2 : Fetch<OrderInfo>
    {
        protected override IDataProvider GetDataProvider()
        {
            return TestEnvironment.GetSqlServerDataProvider();
        }
    }

    [TestFixture]
    public class SqlFetch3 : Fetch<Category>
    {
        protected override IDataProvider GetDataProvider()
        {
            return TestEnvironment.GetSqlServerDataProvider();
        }
    }
}
