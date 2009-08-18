using NUnit.Framework;
using Centro.OpenEntity.DataProviders;
using Centro.OpenEntity.Entities;
using Centro.OpenEntity.Repository;
using Centro.OpenEntity.Specs.Mock.Northwind;
using Centro.OpenEntity.Proxy;

namespace Centro.OpenEntity.Specs.Repository
{
    [TestFixture]
    public class Save : RepositoryTestBase<Product>
    {
        [Test]
        public void With_New_Object_Should_Insert()
        {
            var newProduct = Repository.Create();
            newProduct.Name = "Blamo";
            newProduct.Discontinued = false;
            var itemCountBefore = (int)Repository.FetchScalar(p => p.Id, AggregateFunction.Count);
            Assert.IsTrue(Repository.Save(newProduct));            
            var itemCountAfter = (int)Repository.FetchScalar(p => p.Id, AggregateFunction.Count);            
            Assert.Greater(itemCountAfter, itemCountBefore);

            var entity = EntityProxyFactory.AsEntity(newProduct);

            Assert.IsFalse(entity.IsNew);
            Assert.IsFalse(entity.Fields.State == EntityState.New);

            Repository.Delete(newProduct);
        }

        protected override IDataProvider GetDataProvider()
        {
            return TestEnvironment.GetSqlServerDataProvider();
        }
    }
}
