using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Joins;
using NUnit.Framework;

namespace OpenEntity.Specs.Joins
{
    [TestFixture]
    public class JoinTests
    {
        [Test]
        public void PropertiesShouldBeSetOnNewOneToManyJoin()
        {
            var join = new Join("OriginTable", "OriginColumn", "DestTable", "DestColumn", RelationshipType.OneToMany);

            Assert.AreEqual("DestTable".ToUpper(), join.DestinationTable);
            Assert.AreEqual("DestColumn".ToUpper(), join.DestinationColumn);
            Assert.AreEqual("OriginTable".ToUpper(), join.OriginTable);
            Assert.AreEqual("OriginColumn".ToUpper(), join.OriginColumn);
            Assert.AreEqual(RelationshipType.OneToMany, join.RelationshipType);
        }

        [Test]
        public void PropertiesShouldBeSetOnNewOneToOneJoin()
        {
            var join = new Join("OriginTable", "OriginColumn", "DestTable", "DestColumn", RelationshipType.OneToOne);

            Assert.AreEqual("DestTable".ToUpper(), join.DestinationTable);
            Assert.AreEqual("DestColumn".ToUpper(), join.DestinationColumn);
            Assert.AreEqual("OriginTable".ToUpper(), join.OriginTable);
            Assert.AreEqual("OriginColumn".ToUpper(), join.OriginColumn);
            Assert.AreEqual(RelationshipType.OneToOne, join.RelationshipType);
        }
    }
}
