using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Centro.OpenEntity.Mapping;

namespace Centro.OpenEntity.Specs.Mapping
{
    [TestFixture]
    public class MappingTableTests
    {
        [TestFixtureSetUp]
        public void ClearMappings()
        {
            MappingTable.Clear();
        }

        [Test]        
        public void DefaultMappingConfigShouldNotFindAnyClassMappings()
        {
            foreach (var type in TestEnvironment.EntityTypes)
            {
                Assert.IsNull(MappingTable.FindClassMapping(type));
            }
        }

        [Test]
        public void AddingAssemblyShouldFindAllClassMappings()
        {
            MappingTable.AddAssembly(typeof(TestEnvironment).Assembly);
            foreach (var type in TestEnvironment.EntityTypes)
            {
                Assert.IsNotNull(MappingTable.FindClassMapping(type));
            }
        }

        [Test]
        public void ClearShouldClearSearchedAssemblies()
        {
            MappingTable.AddAssembly(typeof(TestEnvironment).Assembly);
            foreach (var type in TestEnvironment.EntityTypes)
            {
                Assert.IsNotNull(MappingTable.FindClassMapping(type));
            }
            MappingTable.Clear();
            foreach (var type in TestEnvironment.EntityTypes)
            {
                Assert.IsNull(MappingTable.FindClassMapping(type));
            }
        }
    }
}
