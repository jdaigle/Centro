using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Schema;
using OpenEntity.Helpers;
using OpenEntity.Mapping;

namespace OpenEntity.CodeDom
{
    public static class ProxyFactory
    {
        private static Dictionary<Type, Type> proxyTypes = new Dictionary<Type, Type>();

        /// <summary>
        /// Gets the proxy class for the specific target class type.
        /// </summary>
        /// <param name="targetClass">The target class.</param>
        /// <returns></returns>
        public static Type GetProxyClass(Type targetClass)
        {
            ContractUtils.Requires(targetClass != null, "targetClass");

            IClassMapping classMapping = OpenEntity.Mapping.MappingConfig.FindClassMapping(targetClass);
            if (classMapping == null)
                throw new NotSupportedException("Cannot create proxy class for " + targetClass.FullName + " without a class/table mapping.");

            Type proxyType = null;
            if (proxyTypes.ContainsKey(targetClass))
            {
                proxyType = proxyTypes[targetClass];
            }
            else
            {
                var gen = new ProxyGenerator(classMapping);
                proxyType = gen.Build();
                proxyTypes.Add(targetClass, proxyType);
            }
            return proxyType;            
        }
    }
}
