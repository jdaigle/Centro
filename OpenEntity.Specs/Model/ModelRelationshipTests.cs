using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenEntity.Mapping;
using OpenEntity.Repository;
using OpenEntity.Specs.Mock.Northwind;
using OpenEntity.Entities;
using OpenEntity.Specs.Mock;
using System.Diagnostics;

namespace OpenEntity.Specs.Model
{
    [TestFixture]
    public class ModelRelationshipTests
    {
        private IRepository<Product> productRepository;
        private IRepository<Category> categoryRepository;

        [TestFixtureSetUp]
        public void SetupMapping()
        {
            MappingConfiguration.Clear();
            MappingConfiguration.AddAssembly(typeof(TestEnvironment).Assembly);

            productRepository = new RepositoryBase<Product>(TestEnvironment.GetSqlServerDataProvider());
            categoryRepository = new RepositoryBase<Category>(TestEnvironment.GetSqlServerDataProvider());
        }

        [Test]
        public void ShouldFetchRelatedCategoryForProduct()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var products = productRepository.FetchAll(null);
            stopwatch.Stop();
            Trace.WriteLine(string.Format("Spend {0} ms fetching products", stopwatch.ElapsedMilliseconds));
            Assert.IsNotNull(products);
            Assert.IsNotEmpty(products.ToList());
            bool atLeaseOneCategoryFetched = false;
            foreach (var product in products)
            {
                if (product.CategoryId != null)
                {
                    atLeaseOneCategoryFetched = true;
                    var category = product.GetRelatedCategory(categoryRepository);
                    Assert.IsNotNull(category);
                    Assert.IsNotNullOrEmpty(category.Name);
                }
            }
            Assert.IsTrue(atLeaseOneCategoryFetched);
        }
    }
}