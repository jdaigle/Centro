using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Centro.OpenEntity.Joins;
using NUnit.Framework;

namespace Centro.OpenEntity.Specs.Joins
{
    [TestFixture]
    public class JoinSetTests
    {
        [Test]
        public void OriginTablePropertyShouldBeSet()
        {
            var joinSet = new JoinSet("OriginTable");
            Assert.AreEqual("OriginTable".ToUpper(), joinSet.OriginTable);
        }

        [Test]
        public void AddOneToManyShouldAdd()
        {
            var joinSet = new JoinSet("OriginTable");
            var join = joinSet.AddOneToMany("DestTable", "OriginColumn", "DestColumn");
            Assert.IsNotNull(join);            
            Assert.AreEqual(join, joinSet.Get("DestTable"));
            Assert.IsNotNull(joinSet.Any(j => j == join));
        }

        [Test]
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

        [Test]
        public void AddOneToOneShouldAdd()
        {
            var joinSet = new JoinSet("OriginTable");
            var join = joinSet.AddOneToOne("DestTable", "OriginColumn", "DestColumn");
            Assert.IsNotNull(join);
            Assert.AreEqual(join, joinSet.Get("DestTable"));
            Assert.IsNotNull(joinSet.Any(j => j == join));
        }

        [Test]
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

        [Test]
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
