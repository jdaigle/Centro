using System;
using System.Linq;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using OpenEntity.Entities;

namespace OpenEntity.Proxy
{
    public static class EntityProxy
    {
        private static readonly ProxyGenerator generator = new ProxyGenerator();

        public static bool IsEntity(object obj)
        {
            return FindEntityFieldInterceptor(obj) != null;
        }

        private static EntityFieldInterceptor FindEntityFieldInterceptor(object target)
        {
            if (target == null)
            {
                return null;
            }
            var hack = target as IProxyTargetAccessor;
            if (hack == null)
            {
                return null;
            }
            return hack.GetInterceptors().FirstOrDefault(i => i is EntityFieldInterceptor) as EntityFieldInterceptor;
        }

        internal static IEntity AsEntity(object target)
        {
            var entityFieldInterceptor = FindEntityFieldInterceptor(target);
            return entityFieldInterceptor != null ? entityFieldInterceptor.Entity : null;
        }

        public static object MakeEntity(Type targetClass)
        {
            var entityFieldInterceptor = new EntityFieldInterceptor();
            var options = new ProxyGenerationOptions(new EntityFieldProxyGenerationHook());
#if DEBUG
            var proxy = generator.CreateClassProxy(targetClass, options, new CallLoggingInterceptor(), entityFieldInterceptor);
#else
            var proxy = generator.CreateClassProxy(targetClass, options, entityFieldInterceptor);
#endif
            return proxy;
        }
    }
}
