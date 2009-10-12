using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Centro.Extensions;
using System.Runtime.InteropServices;

namespace Centro.Core.Specs.Extensions
{
    public class StringsTests
    {
        [TestFixture]
        public class Matches
        {
            [Test]
            public void Should_Compare_Strings()
            {
                var stringA = "matching string";
                var stringB = "another matching string";

                var matchingResult = stringA.Matches(stringA.ToString());
                var notMatchingResult = stringA.Matches(stringB);

                Assert.IsTrue(matchingResult);
                Assert.IsFalse(notMatchingResult);
            }

            [Test]
            public void Should_Be_Case_Insensitive()
            {
                var stringA = "matching string";
                var stringB = "another matching string";

                var matchingResult = stringA.Matches(stringA.ToString().ToUpperInvariant());
                var notMatchingResult = stringA.Matches(stringB.ToUpperInvariant());

                Assert.IsTrue(matchingResult);
                Assert.IsFalse(notMatchingResult);
            }
        }

        [TestFixture]
        public class MatchesTrimmed
        {
            [Test]
            public void Should_Compare_Strings()
            {
                var stringA = "matching string";
                var stringB = "another matching string";

                var matchingResult = stringA.MatchesTrimmed(stringA.ToString());
                var notMatchingResult = stringA.MatchesTrimmed(stringB);

                Assert.IsTrue(matchingResult);
                Assert.IsFalse(notMatchingResult);
            }

            [Test]
            public void Should_Be_Case_Insensitive()
            {
                var stringA = "matching string";
                var stringB = "another matching string";

                var matchingResult = stringA.MatchesTrimmed(stringA.ToString().ToUpperInvariant());
                var notMatchingResult = stringA.MatchesTrimmed(stringB.ToUpperInvariant());

                Assert.IsTrue(matchingResult);
                Assert.IsFalse(notMatchingResult);
            }

            [Test]
            public void Should_Trim_Beginning_and_End_Of_Strings()
            {
                var stringA = "   matching string    ";
                var stringB = "   another matching string    ";

                var matchingResult = stringA.Matches(stringA.ToString().ToUpperInvariant());
                var notMatchingResult = stringA.Matches(stringB.ToUpperInvariant());

                Assert.IsTrue(matchingResult);
                Assert.IsFalse(notMatchingResult);
            }
        }

        [TestFixture]
        public class MakeSecure
        {
            [Test]
            public void Result_Should_Be_Same_Length_As_Input()
            {
                var unsecureString = "unsecure string";

                var secureString = unsecureString.MakeSecure();

                Assert.AreEqual(unsecureString.Length, secureString.Length);
            }

            [Test]
            public void Result_Should_Be_ReadOnly()
            {
                var unsecureString = "unsecure string";

                var secureString = unsecureString.MakeSecure();

                Assert.IsTrue(secureString.IsReadOnly());
            }

            [Test]
            public void SecureString_Should_Match_Input()
            {
                var unsecureString = "unsecure string";
                var secureString = unsecureString.MakeSecure();

                IntPtr ptr = Marshal.SecureStringToBSTR(secureString);
                string unsecuredString = Marshal.PtrToStringUni(ptr);

                Assert.AreEqual(unsecureString, unsecuredString);
            }
        }
    }
}
