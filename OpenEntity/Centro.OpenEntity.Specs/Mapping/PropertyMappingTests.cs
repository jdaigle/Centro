using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Centro.OpenEntity.Mapping;
using Centro.OpenEntity.Specs.Mock.Northwind;
using Centro.OpenEntity.Extensions;
using Centro.OpenEntity.Specs.Mock;

namespace Centro.OpenEntity.Specs.Mapping
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
