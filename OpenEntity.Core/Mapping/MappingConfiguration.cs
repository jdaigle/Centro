using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace OpenEntity.Mapping
{
    public static class MappingConfiguration
    {
        private static Dictionary<Type, IClassConfiguration> foundClassConfigurations = new Dictionary<Type, IClassConfiguration>();
        private static IList<Assembly> searchAssemblies = new List<Assembly>();

        public static void Clear()
        {
            foundClassConfigurations.Clear();
            searchAssemblies.Clear();
        }

        public static void AddAssembly(Assembly assembly)
        {
            if (!searchAssemblies.Contains(assembly))
                searchAssemblies.Add(assembly);
        }

        public static IClassConfiguration FindClassConfiguration(Type targetClass)
        {
            if (foundClassConfigurations.ContainsKey(targetClass))
                return foundClassConfigurations[targetClass];
            var classConfigurationType = typeof(IClassConfiguration);
            foreach (var assembly in searchAssemblies)
            {
                foreach (Type type in assembly.GetExportedTypes())
                {
                    if (classConfigurationType.IsAssignableFrom(type))
                    {
                        var classConfiguration = (IClassConfiguration)Activator.CreateInstance(type);
                        if (!foundClassConfigurations.ContainsKey(classConfiguration.ClassType))
                            foundClassConfigurations[classConfiguration.ClassType] = classConfiguration;
                    }
                }
            }
            if (foundClassConfigurations.ContainsKey(targetClass))
                return foundClassConfigurations[targetClass];
            return null;
        }

    }
}
