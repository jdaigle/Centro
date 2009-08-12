using System;
using System.Linq;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using OpenEntity.Entities;
using OpenEntity.Mapping;
using System.Diagnostics;

namespace OpenEntity.Proxy
{
    public static class EntityProxyFactory
    {
        private static readonly ProxyGenerator generator = new ProxyGenerator();
        private static readonly ProxyGenerationOptions options = new ProxyGenerationOptions(new EntityFieldProxyGenerationHook());

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

        public static IEntity AsEntity(object target)
        {
            var entityFieldInterceptor = FindEntityFieldInterceptor(target);
            return entityFieldInterceptor != null ? entityFieldInterceptor.Entity : null;
        }

        public static object MakeEntity(Type targetClass)
        {
            var classConfiguration = MappingConfiguration.FindClassConfiguration(targetClass);
            if (classConfiguration == null)
                throw new NotSupportedException("Cannot create proxy class for " + targetClass.FullName + " without a class/table configuration.");

            var entityFieldInterceptor = new EntityFieldInterceptor();
#if DEBUG
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            // TODO determined that running the generator with the ProxyGenerationOptions SIGNIFICANTLY slows down the runtime.
            var proxy = generator.CreateClassProxy(targetClass, options, new CallLoggingInterceptor(), entityFieldInterceptor);
            //var proxy = generator.CreateClassProxy(targetClass, new CallLoggingInterceptor(), entityFieldInterceptor);
            stopwatch.Stop();
            totalTimeSpentCreatingProxies += stopwatch.ElapsedMilliseconds;
            Trace.WriteLine(string.Format("Took {0} ms to create class proxy", stopwatch.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Spent {0} ms to creating class proxies", totalTimeSpentCreatingProxies));
#else
            var proxy = generator.CreateClassProxy(targetClass, options, entityFieldInterceptor);
#endif
            return proxy;
        }

#if DEBUG
        private static long totalTimeSpentCreatingProxies;
#endif
    }
}
