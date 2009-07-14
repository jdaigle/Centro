using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace OpenEntity.Mapping
{
    public static class MappingConfig
    {
        private static Dictionary<Type, IClassMapping> foundMappings = new Dictionary<Type, IClassMapping>();
        private static IList<Assembly> searchAssemblies = new List<Assembly>();

        /// <summary>
        /// Adds a mapping assembly to this config. This assembly will be included in the
        /// search for IClassMapping implementations.
        /// </summary>
        /// <param name="assembly"></param>
        public static void AddAssembly(Assembly assembly)
        {
            if (!searchAssemblies.Contains(assembly))
                searchAssemblies.Add(assembly);
        }

        /// <summary>
        /// Attempts to find the IClassMapping for the specified target class Type.
        /// </summary>
        /// <param name="targetClass"></param>
        /// <returns></returns>
        public static IClassMapping FindClassMapping(Type targetClass)
        {
            if (foundMappings.ContainsKey(targetClass))
                return foundMappings[targetClass];
            var iclassmappingType = typeof(IClassMapping);
            foreach (var assembly in searchAssemblies)
            {
                foreach (Type type in assembly.GetExportedTypes())
                {
                    if (iclassmappingType.IsAssignableFrom(type))
                    {
                        var classMapping = (IClassMapping)Activator.CreateInstance(type);
                        if (!foundMappings.ContainsKey(classMapping.ClassType))
                            foundMappings[classMapping.ClassType] = classMapping;
                    }
                }
            }
            if (foundMappings.ContainsKey(targetClass))
                return foundMappings[targetClass];
            return null;
        }

    }
}
