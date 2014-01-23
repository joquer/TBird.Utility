namespace TBird.Utility.Test
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using NUnit.Framework;

    using TBird.Utility;

    /// <summary>
    /// Test routines for TextUtility class in TBird.Utility.
    /// </summary>
    [TestFixture]
    public class TextUtilityTest
    {
        [Test]
        public void IsMemberOfTest()
        {
            char[] list = { ',', 't', 'r' };

            bool expected = false;
            bool actual = TextUtility.IsMemberOf(list, 'q');
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = TextUtility.IsMemberOf(list, ',');
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = TextUtility.IsMemberOf(null, ',');
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = TextUtility.IsMemberOf(new char[] { ',' }, 't');
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = TextUtility.IsMemberOf(new char[] { ',' }, ',');
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = TextUtility.IsMemberOf(new char[] { }, ',');
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = TextUtility.IsMemberOf(null, ',');
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RemoveQuotesTest()
        {
            string[] inputs = { "asdf", "\"\"asdf", "asdf\"", "a\"s\"d\"f\"", "as\"df" };
            Collection<string> tokens = new Collection<string>();
            foreach (string s in inputs)
                tokens.Add(s);
            IEnumerable<string> results = TextUtility.RemoveQuotes(tokens);
            foreach (string s in results)
            {
                Assert.AreEqual("asdf", s);
            }

            CollectionAssert.AreEquivalent(
                new List<string>() { string.Empty, string.Empty },
                TextUtility.RemoveQuotes(new string[] { null, string.Empty }.AsEnumerable()).ToList());
            Assert.AreEqual(TextUtility.RemoveQuotes(null), Enumerable.Empty<string>());
        }

        [Test]
        public void ParseIntTest()
        {
            string value = "123456";
            int expected = 123456;
            int actual = TextUtility.ParseInt32(value);
            Assert.AreEqual(expected, actual);

            value = "123,456";
            actual = TextUtility.ParseInt32(value);
            Assert.AreEqual(expected, actual);

            expected = 0;
            value = "24bcdef456";
            actual = TextUtility.ParseInt32(value);
            Assert.AreEqual(expected, actual);

            expected = 0;
            value = null;
            actual = TextUtility.ParseInt32(value);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseLongTest()
        {
            string value = "123456789";
            long expected = 123456789;
            long actual = TextUtility.ParseInt64(value);
            Assert.AreEqual(expected, actual);

            value = "123,456,789";
            actual = TextUtility.ParseInt64(value);
            Assert.AreEqual(expected, actual);

            value = "123M";
            expected = 123000000;
            actual = TextUtility.ParseInt64(value);
            Assert.AreEqual(expected, actual);

            value = "1234M";
            expected = 1234000000;
            actual = TextUtility.ParseInt64(value);
            Assert.AreEqual(expected, actual);

            value = "1,234M";
            expected = 1234000000;
            actual = TextUtility.ParseInt64(value);
            Assert.AreEqual(expected, actual);

            value = "1=234M";
            expected = 0;
            actual = TextUtility.ParseInt64(value);
            Assert.AreEqual(expected, actual);

            value = "123B";
            expected = 123000000000;
            actual = TextUtility.ParseInt64(value);
            Assert.AreEqual(expected, actual);

            value = "1234B";
            expected = 1234000000000;
            actual = TextUtility.ParseInt64(value);
            Assert.AreEqual(expected, actual);

            value = "1,234B";
            expected = 1234000000000;
            actual = TextUtility.ParseInt64(value);
            Assert.AreEqual(expected, actual);

            expected = 0;
            value = "1-234B";
            expected = 0;
            actual = TextUtility.ParseInt64(value);
            Assert.AreEqual(expected, actual);

            value = "24bcdef456";
            actual = TextUtility.ParseInt64(value);
            Assert.AreEqual(expected, actual);

            expected = 0;
            value = null;
            actual = TextUtility.ParseInt64(value);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseDecimalTest()
        {
            string value = "1.0";
            decimal expected = 1;
            decimal actual;
            actual = TextUtility.ParseDecimal(value);
            Assert.AreEqual(expected, actual);

            value = "1234.5678";
            expected = 1234.5678M;
            actual = TextUtility.ParseDecimal(value);
            Assert.AreEqual(expected, actual);

            value = "12.5%";
            expected = 12.5M;
            actual = TextUtility.ParseDecimal(value);
            Assert.AreEqual(expected, actual);

            value = "1234-5678";
            expected = 0;
            actual = TextUtility.ParseDecimal(value);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseDoubleTest()
        {
            string value = "1.0";
            double expected = 1F;
            double actual;
            actual = TextUtility.ParseDouble(value);
            Assert.AreEqual(expected, actual);

            value = "1234.5678";
            expected = 1234.5678;
            actual = TextUtility.ParseDouble(value);
            Assert.AreEqual(expected, actual);

            value = "12.5%";
            expected = 12.5;
            actual = TextUtility.ParseDouble(value);
            Assert.AreEqual(expected, actual);

            value = "1234-5678";
            expected = 0;
            actual = TextUtility.ParseDouble(value);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateIndent_CreateIndentString_MatchesLength()
        {
            Assert.AreEqual(string.Empty, TextUtility.CreateIndentString(0));
            Assert.AreEqual("  ", TextUtility.CreateIndentString(2));
            Assert.AreEqual("    ", TextUtility.CreateIndentString(4));
        }
    }
}
