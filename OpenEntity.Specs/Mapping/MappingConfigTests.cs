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
            MappingConfig.Clear();
        }

        [Test]        
        public void DefaultMappingConfigShouldNotFindAnyClassMappings()
        {
            foreach (var type in TestEnvironment.EntityTypes)
            {
                Assert.IsNull(MappingConfig.FindClassMapping(type));
            }
        }

        [Test]
        public void AddingAssemblyShouldFindAllClassMappings()
        {
            MappingConfig.AddAssembly(typeof(TestEnvironment).Assembly);
            foreach (var type in TestEnvironment.EntityTypes)
            {
                Assert.IsNotNull(MappingConfig.FindClassMapping(type));
            }
        }

        [Test]
        public void ClearShouldClearSearchedAssemblies()
        {
            MappingConfig.AddAssembly(typeof(TestEnvironment).Assembly);
            foreach (var type in TestEnvironment.EntityTypes)
            {
                Assert.IsNotNull(MappingConfig.FindClassMapping(type));
            }
            MappingConfig.Clear();
            foreach (var type in TestEnvironment.EntityTypes)
            {
                Assert.IsNull(MappingConfig.FindClassMapping(type));
            }
        }
    }
}
