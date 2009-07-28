using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenEntity.Joins;

namespace OpenEntity.Tests.Joins
{
    [TestClass]
    public class JoinTests
    {
        public JoinTests()
        {
        }
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void PropertiesShouldBeSetOnNewJoin()
        {
            var join = new Join("OriginTable", "OriginColumn", "DestTable", "DestColumn", RelationshipType.OneToMany);
            Assert.AreEqual("DestTable".ToUpper(), join.DestinationTable);
            Assert.AreEqual("DestColumn".ToUpper(), join.DestinationColumn);
            Assert.AreEqual("OriginTable".ToUpper(), join.OriginTable);
            Assert.AreEqual("OriginColumn".ToUpper(), join.OriginColumn);
            Assert.AreEqual(RelationshipType.OneToMany, join.RelationshipType);

            join = new Join("OriginTable", "OriginColumn", "DestTable", "DestColumn", RelationshipType.OneToOne);
            Assert.AreEqual("DestTable".ToUpper(), join.DestinationTable);
            Assert.AreEqual("DestColumn".ToUpper(), join.DestinationColumn);
            Assert.AreEqual("OriginTable".ToUpper(), join.OriginTable);
            Assert.AreEqual("OriginColumn".ToUpper(), join.OriginColumn);
            Assert.AreEqual(RelationshipType.OneToOne, join.RelationshipType);
        }
    }
}
