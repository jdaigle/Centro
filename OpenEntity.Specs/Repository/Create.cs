using NUnit.Framework;
using OpenEntity.Entities;
using OpenEntity.Tests.Mock.Northwind;
using OpenEntity.DataProviders;
using OpenEntity.Model;

namespace OpenEntity.Specs.Repository
{
    public abstract class Create<TModelType> : RepositoryTestBase<TModelType> where TModelType : IDomainObject
    {
        [Test]
        public void ShouldReturnIEntity()
        {
            var entity = Repository.Create();
            Assert.IsInstanceOf(typeof(IEntity), entity);
        }

        [Test]
        public void ShouldReturnTModelType()
        {
            var entity = Repository.Create();
            Assert.IsInstanceOf(typeof(TModelType), entity);
        }

        [Test]
        public void ShouldHaveEntityTable()
        {
            var entity = Repository.Create() as IEntity;
            Assert.IsNotNull(entity.Table);
            Assert.IsNotNullOrEmpty(entity.Table.Name);
        }

        [Test]
        public void ShouldHaveFields()
        {
            var entity = Repository.Create() as IEntity;
            Assert.IsNotNull(entity.Fields);
        }

        [Test]
        public void ShouldBeNew()
        {
            var entity = Repository.Create() as IEntity;
            Assert.IsTrue(entity.IsNew);
        }

        [Test]
        public void ShouldNotBeDirty()
        {
            var entity = Repository.Create() as IEntity;
            Assert.IsFalse(entity.IsDirty);
        }

        [Test]
        public void ShouldHaveEntityStateBeNew()
        {
            var entity = Repository.Create() as IEntity;
            Assert.AreEqual(entity.Fields.State, EntityState.New);
        }
    }

    [TestFixture]
    public class SqlCreate1 : Create<Product>
    {
        protected override IDataProvider GetDataProvider()
        {
            return TestEnvironment.GetSqlServerDataProvider();
        }
    }

    [TestFixture]
    public class SqlCreate2 : Create<Category>
    {
        protected override IDataProvider GetDataProvider()
        {
            return TestEnvironment.GetSqlServerDataProvider();
        }
    }
}
