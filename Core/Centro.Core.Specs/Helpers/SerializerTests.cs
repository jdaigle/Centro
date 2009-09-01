using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Centro.Core.Helpers;

namespace Centro.Core.Specs.Helpers
{
    public class SerializerTests
    {
        public class Foo
        {
            public string StringField;
            public DateTime DateField;
            public int IntegerField;

            public override bool Equals(object obj)
            {
                var foo = obj as Foo;
                return foo != null &&
                    foo.StringField == this.StringField &&
                    foo.IntegerField == this.IntegerField &&
                    foo.DateField == this.DateField;
            }
        }

        [TestFixture]
        public class ConvertToXml
        {
            [Test]
            public void Should_Return_String()
            {
                var foo = new Foo() { DateField = DateTime.Now, StringField = "some string", IntegerField = 5 };

                var result = Serializer.ConvertToXml<Foo>(foo);

                Assert.IsNotNull(result);
            }
        }

        [TestFixture]
        public class ConvertFromXml
        {
            [Test]
            public void Should_Deserialize_Object()
            {
                var foo = new Foo() { DateField = DateTime.Now, StringField = "some string", IntegerField = 5 };
                var serialized = Serializer.ConvertToXml<Foo>(foo);

                var result = Serializer.ConvertFromXml<Foo>(serialized);

                Assert.IsNotNull(result);
                Assert.AreEqual(foo, result);
            }
        }
    }
}
