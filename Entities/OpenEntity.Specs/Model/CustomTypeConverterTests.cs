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
using OpenEntity.Proxy;

namespace OpenEntity.Specs.Model
{
    [TestFixture]
    public class CustomTypeConverterTests
    {
        private IRepository<Supplier> repository;

        [TestFixtureSetUp]
        public void SetupMapping()
        {
            MappingTable.Clear();
            MappingTable.AddAssembly(typeof(TestEnvironment).Assembly);

            repository = new RepositoryBase<Supplier>(TestEnvironment.GetSqlServerDataProvider());
        }

        [Test]
        public void ShouldFetchCorrectType()
        {
            var suppliers = repository.FetchAll(null);
            Assert.IsNotNull(suppliers);
            Assert.IsNotEmpty(suppliers.ToList());
            var supplier = suppliers.First();
            var supplierEntity = EntityProxyFactory.AsEntity(supplier);
            Assert.IsNotNull(supplier.Phone);
            Assert.IsNotNullOrEmpty(supplier.Phone.ToString());
            Assert.AreEqual(supplier.Phone.ToString(), supplierEntity.Fields["Phone"].CurrentValue.ToString());
        }

        [Test]
        public void ShouldSetCorrectType()
        {
            var suppliers = repository.FetchAll(null);
            Assert.IsNotNull(suppliers);
            Assert.IsNotEmpty(suppliers.ToList());
            var supplier = suppliers.First();
            var supplierEntity = EntityProxyFactory.AsEntity(supplier);
            var fakePhone = new SimplePhoneNumber("123-456-1230");
            supplier.Phone = fakePhone;
            Assert.IsNotNull(supplier.Phone);
            Assert.IsNotNullOrEmpty(supplier.Phone.ToString());
            Assert.AreEqual(fakePhone.ToString(), supplier.Phone.ToString());
            Assert.AreEqual(fakePhone.ToString(), supplierEntity.Fields["Phone"].CurrentValue.ToString());
        }
    }
}
