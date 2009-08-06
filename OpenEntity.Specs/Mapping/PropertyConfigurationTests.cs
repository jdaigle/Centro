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
    public class PropertyConfigurationTests
    {

        [TestFixtureSetUp]
        public void SetupMapping()
        {
            MappingConfiguration.Clear();
            MappingConfiguration.AddAssembly(typeof(TestEnvironment).Assembly);
        }

        [Test]
        public void CustomTypeShouldBeSet()
        {
            var classConfiguration = MappingConfiguration.FindClassConfiguration(typeof(Supplier));
            Assert.IsNotNull(classConfiguration);
            var phoneProperty = classConfiguration.Properties.FirstOrDefault(p => p.Name.Matches("Phone"));
            Assert.IsNotNull(phoneProperty);
            Assert.IsNotNull(phoneProperty.CustomTypeConverter);
            Assert.IsTrue(phoneProperty.CustomTypeConverter is SimplePhoneNumberConverter);
        }
    }
}
