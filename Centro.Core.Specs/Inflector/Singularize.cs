using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Centro.Inflector;

namespace Centro.Core.Specs.Inflector
{
    [TestFixture]
    public class Singularize
    {
        [Test]
        public void TestCases()
        {
            foreach (var toPlural in Pluralize.testCases)
            {
                Assert.AreEqual(toPlural.Key, toPlural.Value.Singularize());
            }
        }

    }
}
