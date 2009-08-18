using NUnit.Framework;
using OpenEntity.Entities;
using OpenEntity.Specs.Mock.Northwind;
using OpenEntity.DataProviders;
using OpenEntity.Model;
using OpenEntity.Proxy;

namespace OpenEntity.Specs.Repository
{
    public abstract class Fetch<TModelType> : RepositoryTestBase<TModelType> where TModelType : IDomainObject
    {
        [Test]
        public void Should_Return_Entity()
        {
            var instance = Repository.Fetch(null);
            var entity = EntityProxyFactory.AsEntity(instance);
            Assert.IsNotNull(entity);
            Assert.IsInstanceOf(typeof(IEntity), entity);
            Assert.IsInstanceOf(typeof(TModelType), instance);
        }

        [Test]
        public void Should_Return_Fetched_Entity()
        {
            var instance = Repository.Fetch(null);
            var entity = EntityProxyFactory.AsEntity(instance);
            Assert.AreEqual(entity.Fields.State, EntityState.Fetched);
        }

        [Test]
        public void Should_Return_Not_Dirty_Entity()
        {
            var instance = Repository.Fetch(null);
            var entity = EntityProxyFactory.AsEntity(instance);
            Assert.IsFalse(entity.IsDirty);
        }

        [Test]
        public void Should_Return_Entity_With_Field_Values()
        {
            var instance = Repository.Fetch(null);
            var entity = EntityProxyFactory.AsEntity(instance);
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
