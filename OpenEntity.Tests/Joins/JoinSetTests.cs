using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenEntity.Joins;

namespace OpenEntity.Tests.Joins
{
    [TestClass]
    public class JoinSetTests
    {
        public JoinSetTests()
        {
        }
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void OriginTablePropertyShouldBeSet()
        {
            var joinSet = new JoinSet("OriginTable");
            Assert.AreEqual("OriginTable".ToUpper(), joinSet.OriginTable);
        }

        [TestMethod]
        public void AddOneToManyShouldAdd()
        {
            var joinSet = new JoinSet("OriginTable");
            var join = joinSet.AddOneToMany("DestTable", "OriginColumn", "DestColumn");
            Assert.IsNotNull(join);            
            Assert.AreEqual(join, joinSet.Get("DestTable"));
            Assert.IsNotNull(joinSet.Any(j => j == join));
        }

        [TestMethod]
        public void AddDuplicateOneToManyShouldFail()
        {
            var joinSet = new JoinSet("OriginTable");
            joinSet.AddOneToMany("DestTable", "OriginColumn", "DestColumn");
            try
            {
                joinSet.AddOneToMany("DestTable", "OriginColumn", "DestColumn");
            }
            catch (NotSupportedException)
            {
                // Expected
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AddOneToOneShouldAdd()
        {
            var joinSet = new JoinSet("OriginTable");
            var join = joinSet.AddOneToOne("DestTable", "OriginColumn", "DestColumn");
            Assert.IsNotNull(join);
            Assert.AreEqual(join, joinSet.Get("DestTable"));
            Assert.IsNotNull(joinSet.Any(j => j == join));
        }

        [TestMethod]
        public void AddDuplicateOneToOneShouldFail()
        {
            var joinSet = new JoinSet("OriginTable");
            joinSet.AddOneToOne("DestTable", "OriginColumn", "DestColumn");
            try
            {
                joinSet.AddOneToOne("DestTable", "OriginColumn", "DestColumn");
            }
            catch (NotSupportedException)
            {
                // Expected
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void CountPropertyShouldIncrement()
        {
            var joinSet = new JoinSet("OriginTable");
            Assert.AreEqual(0, joinSet.Count);
            joinSet.AddOneToMany("DestTable", "OriginColumn", "DestColumn");
            Assert.AreEqual(1, joinSet.Count);
            joinSet.AddOneToOne("DestTable2", "OriginColumn", "DestColumn");
            Assert.AreEqual(2, joinSet.Count);
        }
    }
}
