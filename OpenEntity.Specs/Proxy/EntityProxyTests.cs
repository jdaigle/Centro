using System;
using System.Linq;
using Castle.Core.Interceptor;
using NUnit.Framework;
using OpenEntity.Proxy;

namespace OpenEntity.Specs.Proxy
{
    [TestFixture]
    public class EntityProxyTests
    {
        [Test]
        public void IsEntity_should_be_false_for_objects_created_with_ctor()
        {
            var transientPet = new Pet();
            Assert.False(EntityProxy.IsEntity(transientPet));
        }

        [Test]
        public void IsEntity_should_be_false_for_objects_created_with_MakeEntity()
        {
            var proxyPet = EntityProxy.MakeEntity(typeof(Pet));
            Assert.True(EntityProxy.IsEntity(proxyPet));
        }

        [Test]
        public void EntityProxy_should_create_classes_with_nonVirtual_methods()
        {
            var pet = (WithNonVirtualMethod)EntityProxy.MakeEntity(typeof(WithNonVirtualMethod));
            pet.Name = "Rex";
            pet.NonVirtualMethod();
        }

        [Test]
        public void EntityProxy_should__create_classes_with_nonVirtual_properties()
        {
            var pet = (WithNonVirtualProperty)EntityProxy.MakeEntity(typeof(WithNonVirtualProperty));
            pet.Name = "Rex";
            pet.NonVirtualProperty = 5;
            var value = pet.NonVirtualProperty;
        }

        [Test]
        public void EntityProxy_should_not_intercept_normal_methods()
        {
            var pet = (Pet)EntityProxy.MakeEntity(typeof(Pet));
            var notUsed = pet.GetHashCode(); //should not intercept
            var alsoNotUsed = pet.Equals(null); //should not intercept
            var interceptedMethodsCount = GetInterceptedMethodsCountFor<EntityFieldInterceptor>(pet);
            Assert.AreEqual(0, interceptedMethodsCount);
        }

        [Test]
        public void EntityProxy_should_intercept_property_setters()
        {
            var pet = (Pet)EntityProxy.MakeEntity(typeof(Pet));
            pet.Age = 5; //should intercept
            var interceptedMethodsCount = GetInterceptedMethodsCountFor<EntityFieldInterceptor>(pet);
            Assert.AreEqual(1, interceptedMethodsCount);
        }

        [Test]
        public void EntityProxy_should_intercept_property_getters()
        {
            var pet = (Pet)EntityProxy.MakeEntity(typeof(Pet));
            var age = pet.Age; //should intercept
            var interceptedMethodsCount = GetInterceptedMethodsCountFor<EntityFieldInterceptor>(pet);
            Assert.AreEqual(1, interceptedMethodsCount);
        }

        [Test]
        public void DynProxyGetTarget_should_return_proxy_itself()
        {
            var pet = EntityProxy.MakeEntity(typeof(Pet));
            var hack = pet as IProxyTargetAccessor;
            Assert.NotNull(hack);
            Assert.AreSame(pet, hack.DynProxyGetTarget());
        }

        [Test]
        public void EntityProxy_should_log_getters_and_setters()
        {
            var pet = (Pet)EntityProxy.MakeEntity(typeof(Pet));
            pet.Age = 4;
            var age = pet.Age;
            int logsCount = GetInterceptedMethodsCountFor<CallLoggingInterceptor>(pet);
            int entityCount = GetInterceptedMethodsCountFor<EntityFieldInterceptor>(pet);
            Assert.AreEqual(2, logsCount);
            Assert.AreEqual(2, entityCount);
        }

#if DEBUG
        [Test]
        public void EntityProxy_should_not_intercept_methods()
        {

            var pet = EntityProxy.MakeEntity(typeof(Pet));
            pet.ToString();
            int logsCount = GetInterceptedMethodsCountFor<CallLoggingInterceptor>(pet);
            int entityCount = GetInterceptedMethodsCountFor<EntityFieldInterceptor>(pet);

            // base implementation of ToString calls each property getter, that we intercept
            // so there will be 3 calls if method is not intercepter, otherwise 4.
            Assert.AreEqual(3, logsCount);
            Assert.AreEqual(3, entityCount);
        }
#endif

        [Test]        
        public void EntityProxy_should_not_hold_any_reference_to_created_objects()
        {
            var pet = EntityProxy.MakeEntity(typeof(Pet));
            var petWeakReference = new WeakReference(pet, false);
            pet = null;
            GC.Collect();
            Assert.False(petWeakReference.IsAlive, "Object should have been collected");
        }


#if DEBUG
        private int GetInterceptedMethodsCountFor<TInterceptor>(object entity)
            where TInterceptor : IInterceptor, IHasCount
        {
            Assert.True(EntityProxy.IsEntity(entity));

            var hack = entity as IProxyTargetAccessor;
            Assert.NotNull(hack);
            var loggingInterceptor = hack.GetInterceptors().
                                         Where(i => i is TInterceptor).
                                         Single() as IHasCount;
            return loggingInterceptor.Count;
        }
#endif
    }

    public class Pet
    {
        public virtual int Id { get; private set; }
        public virtual string Name { get; set; }
        public virtual int Age { get; set; }
        public virtual bool Deceased { get; set; }

        public override string ToString()
        {
            return string.Format("Name: {0}, Age: {1}, Deceased: {2}", Name, Age, Deceased);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class WithNonVirtualProperty : Pet
    {
        public int NonVirtualProperty { get; set; }
    }

    public class WithNonVirtualMethod : Pet
    {
        public int NonVirtualMethod()
        {
            return Name.Length;
        }
    }
}
