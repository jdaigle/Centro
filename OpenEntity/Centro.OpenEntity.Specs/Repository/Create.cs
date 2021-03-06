﻿using NUnit.Framework;
using Centro.OpenEntity.Entities;
using Centro.OpenEntity.Specs.Mock.Northwind;
using Centro.OpenEntity.DataProviders;
using Centro.OpenEntity.Model;
using Centro.OpenEntity.Proxy;

namespace Centro.OpenEntity.Specs.Repository
{
    public abstract class Create<TModelType> : RepositoryTestBase<TModelType> where TModelType : IDomainObject
    {
        [Test]
        public void Should_Return_ProxyObject()
        {
            var instance = Repository.Create();
            Assert.IsTrue(EntityProxyFactory.IsEntity(instance));
        }

        [Test]
        public void Should_Return_TModelType()
        {
            var instance = Repository.Create();
            Assert.IsInstanceOf(typeof(TModelType), instance);
        }

        [Test]
        public void Should_Return_Initialized_Entity()
        {
            var instance = Repository.Create();
            var entity = EntityProxyFactory.AsEntity(instance);
            Assert.IsNotNull(entity.Table);
            Assert.IsNotNullOrEmpty(entity.Table.Name);
            Assert.IsNotNull(entity.Fields);
        }

        [Test]
        public void Should_Return_New_Entity()
        {
            var instance = Repository.Create();
            var entity = EntityProxyFactory.AsEntity(instance);
            Assert.IsTrue(entity.IsNew);
        }

        [Test]
        public void Should_Return_Not_Dirty_Entity()
        {
            var instance = Repository.Create();
            var entity = EntityProxyFactory.AsEntity(instance);
            Assert.IsFalse(entity.IsDirty);
        }
    }

    [TestFixture]
    public class SqlCreate1 : Create<Product>
    {
        protected override IDataProvider GetDataProvider()
        {
            return TestEnvironment.GetSqlServerDataProvider();
        }

        [Test]
        public void Should__Throw()
        {
            var instance = Repository.Create();
            Assert.Throws<InvalidFieldReadException>(delegate { string.IsNullOrEmpty(instance.Name); });
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
