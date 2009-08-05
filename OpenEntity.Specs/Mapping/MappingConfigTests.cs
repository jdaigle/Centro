using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenEntity.Mapping;

namespace OpenEntity.Specs.Mapping
{
    [TestFixture]
    public class MappingConfigTests
    {
        [TestFixtureSetUp]
        public void ClearMappings()
        {
            MappingConfiguration.Clear();
        }

        [Test]        
        public void DefaultMappingConfigShouldNotFindAnyClassMappings()
        {
            foreach (var type in TestEnvironment.EntityTypes)
            {
                Assert.IsNull(MappingConfiguration.FindClassConfiguration(type));
            }
        }

        [Test]
        public void AddingAssemblyShouldFindAllClassMappings()
        {
            MappingConfiguration.AddAssembly(typeof(TestEnvironment).Assembly);
            foreach (var type in TestEnvironment.EntityTypes)
            {
                Assert.IsNotNull(MappingConfiguration.FindClassConfiguration(type));
            }
        }

        [Test]
        public void ClearShouldClearSearchedAssemblies()
        {
            MappingConfiguration.AddAssembly(typeof(TestEnvironment).Assembly);
            foreach (var type in TestEnvironment.EntityTypes)
            {
                Assert.IsNotNull(MappingConfiguration.FindClassConfiguration(type));
            }
            MappingConfiguration.Clear();
            foreach (var type in TestEnvironment.EntityTypes)
            {
                Assert.IsNull(MappingConfiguration.FindClassConfiguration(type));
            }
        }
    }
}
