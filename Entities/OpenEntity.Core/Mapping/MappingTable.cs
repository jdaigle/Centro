using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Centro.OpenEntity.Mapping
{
    public static class MappingTable
    {
        private static Dictionary<Type, IClassMapping> foundClassMappings = new Dictionary<Type, IClassMapping>();
        private static IList<Assembly> searchAssemblies = new List<Assembly>();

        public static void Clear()
        {
            foundClassMappings.Clear();
            searchAssemblies.Clear();
        }

        public static void AddAssembly(Assembly assembly)
        {
            if (!searchAssemblies.Contains(assembly))
                searchAssemblies.Add(assembly);
        }

        public static IClassMapping FindClassMapping(Type targetClass)
        {
            if (foundClassMappings.ContainsKey(targetClass))
                return foundClassMappings[targetClass];
            var classMappingType = typeof(IClassMapping);
            foreach (var assembly in searchAssemblies)
            {
                foreach (Type type in assembly.GetExportedTypes())
                {
                    if (classMappingType.IsAssignableFrom(type))
                    {
                        var classMapping = (IClassMapping)Activator.CreateInstance(type);
                        if (!foundClassMappings.ContainsKey(classMapping.ClassType))
                            foundClassMappings[classMapping.ClassType] = classMapping;
                    }
                }
            }
            if (foundClassMappings.ContainsKey(targetClass))
                return foundClassMappings[targetClass];
            return null;
        }

    }
}
