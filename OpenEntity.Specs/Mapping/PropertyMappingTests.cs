using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenEntity.Mapping;
using OpenEntity.Specs.Mock.Northwind;
using OpenEntity.Extensions;
using OpenEntity.Specs.Mock;

namespace OpenEntity.Specs.Mapping
{
    [TestFixture]
    public class PropertyMappingTests
    {

        [TestFixtureSetUp]
        public void SetupMapping()
        {
            MappingTable.Clear();
            MappingTable.AddAssembly(typeof(TestEnvironment).Assembly);
        }

        [Test]
        public void CustomTypeShouldBeSet()
        {
            var classMapping = MappingTable.FindClassMapping(typeof(Supplier));
            Assert.IsNotNull(classMapping);
            var phoneProperty = classMapping.Properties.FirstOrDefault(p => p.Name.Matches("Phone"));
            Assert.IsNotNull(phoneProperty);
            Assert.IsNotNull(phoneProperty.CustomTypeConverter);
            Assert.IsTrue(phoneProperty.CustomTypeConverter is SimplePhoneNumberConverter);
        }
    }
}
