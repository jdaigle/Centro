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
            MappingTable.Clear();
            MappingTable.AddAssembly(typeof(TestEnvironment).Assembly);

            productRepository = new RepositoryBase<Product>(TestEnvironment.GetSqlServerDataProvider());
            categoryRepository = new RepositoryBase<Category>(TestEnvironment.GetSqlServerDataProvider());
        }

        [Test]
        public void Should_Fetch_Related_Category_For_Product()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var products = productRepository.FetchAll(null);
            stopwatch.Stop();
            Trace.WriteLine(string.Format("Spend {0} ms fetching products", stopwatch.ElapsedMilliseconds));
            stopwatch.Reset();
            Assert.IsNotNull(products);
            Assert.IsNotEmpty(products.ToList());
            int productCategoryFetchCount = 0;
            foreach (var product in products)
            {
                var category = product.Category;
                if (category != null)
                {
                    productCategoryFetchCount++;
                    Assert.IsNotNull(category);
                    Assert.IsNotNullOrEmpty(category.Name);
                }
            }
            Trace.WriteLine(string.Format("Fetched {0} product categories from {1} products", productCategoryFetchCount, products.Count));
            Assert.Greater(productCategoryFetchCount, 0);
        }

        [Test]
        public void Should_Cache_Related_Categories()
        {
            var products = productRepository.FetchAll(null);

            Assert.IsNotNull(products);
            Assert.IsNotEmpty(products.ToList());

            foreach (var product in products)
            {
                var category = product.Category;
                if (category != null)
                {
                    Assert.AreSame(category, product.Category);
                }
            }
        }

        [Test]
        public void Should_Clear_Cache_Of_Related_Categories_After_Reload()
        {
            var products = productRepository.FetchAll(null);

            Assert.IsNotNull(products);
            Assert.IsNotEmpty(products.ToList());

            foreach (var product in products)
            {
                var category = product.Category;
                if (category != null)
                {
                    Assert.AreSame(category, product.Category);
                    productRepository.Reload(product);
                    Assert.AreNotEqual(category, product.Category);
                }
            }
        }

        [Test]
        public void Should_Set_Related_Category_For_Product()
        {
            var products = productRepository.FetchAll(null);
            var categories = categoryRepository.FetchAll(null);

            var category1 = categories.FirstOrDefault(x => x.Id == 1);
            var category2 = categories.FirstOrDefault(x => x.Id == 2);

            Assert.IsNotNull(products);
            Assert.IsNotEmpty(products.ToList());
            Assert.IsNotNull(category1);
            Assert.IsNotNull(category2);

            foreach (var product in products)
            {
                var category = product.Category;

                if (category != null)
                {
                    if (category.Id == 1)
                    {
                        product.Category = category2;
                        Assert.IsTrue(productRepository.Save(product));
                    }
                    else if (category.Id == 2)
                    {
                        product.Category = category1;
                        Assert.IsTrue(productRepository.Save(product));
                    }
                }
            }
        }
    }
}