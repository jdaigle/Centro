using NUnit.Framework;
using OpenEntity.DataProviders;
using OpenEntity.Entities;
using OpenEntity.Repository;
using OpenEntity.Specs.Mock.Northwind;

namespace OpenEntity.Specs.Repository
{
    [TestFixture]
    public class Save : RepositoryTestBase<Product>
    {
        [Test]
        public void SavingNewObjectShouldInsert()
        {
            var newProduct = Repository.Create();
            newProduct.Name = "Blamo";
            newProduct.Discontinued = false;
            var itemCountBefore = (int)Repository.FetchScalar(p => p.Id, AggregateFunction.Count);
            Assert.IsTrue(Repository.Save(newProduct));            
            var itemCountAfter = (int)Repository.FetchScalar(p => p.Id, AggregateFunction.Count);            
            Assert.Greater(itemCountAfter, itemCountBefore);

            Assert.IsFalse((newProduct as IEntity).IsNew);
            Assert.IsFalse((newProduct as IEntity).Fields.State == EntityState.New);

            Repository.Delete(newProduct);
        }

        protected override IDataProvider GetDataProvider()
        {
            return TestEnvironment.GetSqlServerDataProvider();
        }
    }
}
