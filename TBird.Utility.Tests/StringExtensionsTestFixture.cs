using TBird.Utility;
using System;

namespace TBird.Utility.Test
{
    using NUnit.Framework;

    /// <summary>
    /// Unit test for the StringExtensions class.
    /// </summary>
    [TestFixture]
    public class StringExtensionsTest
    {
        private class TestSet
        {
            public string TestString { get; set; }
            public string ResultString { get; set; }
            public int TestNumberLines { get; set; }
            public int NumberLinesToRemove { get; set; }
            public int ResultLines { get; set; }

            public TestSet(string test, string result, int testLines, int nLines, int resultLines)
            {
                TestString = test;
                ResultString = result;
                TestNumberLines = testLines;
                NumberLinesToRemove = nLines;
                ResultLines = resultLines;
            }
        }

        private TestSet[] tests =
            {
                new TestSet("Line 1" + Environment.NewLine
                            + "Line 2" + Environment.NewLine
                            + "Line 3" + Environment.NewLine
                            + "Line 4" + Environment.NewLine,
                            "Line 3" + Environment.NewLine
                            + "Line 4" + Environment.NewLine,
                            4, 2, 2),
                new TestSet("Line 1" + Environment.NewLine
                            + "Line 2" + Environment.NewLine
                            + "Line 3" + Environment.NewLine
                            + "Line 4" + Environment.NewLine,
                            string.Empty,
                            4, 6, 0),
                new TestSet(Environment.NewLine,
                            string.Empty,
                            1, 1, 0),
                new TestSet("Line 1" + Environment.NewLine
                            + "Line 2" + Environment.NewLine
                            + "Line 3",
                            "Line 3",
                            3, 2, 1),
                new TestSet(string.Empty,
                            string.Empty,
                            0, 1, 0),
                new TestSet("Line 1" + Environment.NewLine
                            + "Line 2" + Environment.NewLine
                            + "Line 3",
                            string.Empty,
                            3, 3, 0),
                new TestSet(string.Empty,
                            string.Empty,
                            0, 1, 0),
            };

        [Test]
        public void CountLinesTest()
        {
            foreach (TestSet ts in tests)
            {
                int actual = ts.TestString.CountLines();
                Assert.AreEqual(ts.TestNumberLines, actual);
            }
        }

        [Test]
        public void RemoveLinesFromBeginningTest()
        {
            foreach (TestSet ts in tests)
            {
                string actual = ts.TestString.RemoveLinesFromBeginning(ts.NumberLinesToRemove);
                Assert.AreEqual(ts.ResultString, actual);
                Assert.AreEqual(ts.ResultLines, actual.CountLines());
            }
        }

        private class NumericResults
        {
            public string TestString { get; private set; }

            public bool Result { get; private set; }

            public NumericResults(string testString, bool result)
            {
                this.TestString = testString;
                this.Result = result;
            }
        }

        private readonly NumericResults[] _numericTests =
            {
                new NumericResults("01245", true),
                new NumericResults("a1245", false),
                new NumericResults("0124a", false),
                new NumericResults("01s2s4f5", false),
                new NumericResults("0000000000000000000", true),
            };


        [Test]
        public void IsNumericTest()
        {
            foreach (NumericResults result in _numericTests)
            {
                Assert.AreEqual(result.TestString.IsNumeric(), result.Result);
            }
        }

        private class TruncateResult
        {
            public string TestString { get; private set; }

            public int Length { get; private set; }

            public string ResultString { get; set; }

            public TruncateResult(string testString, int length, string resultString)
            {
                this.TestString = testString;
                this.Length = length;
                this.ResultString = resultString;
            }
        }

        private readonly TruncateResult[] truncateResults =
            {
                new TruncateResult("abcdefg", 4, "abcd"),
                new TruncateResult("abcdefg", 20, "abcdefg"),
                new TruncateResult("abcdefg", 0, ""),
                new TruncateResult(null, 0, ""), 
                new TruncateResult(string.Empty, 0, ""), 
            };

        [Test]
        public void TruncateStringTest()
        {
            foreach (TruncateResult result in this.truncateResults)
            {
                Assert.AreEqual(result.TestString.Truncate(result.Length),
                    result.ResultString);
            }
        }

        [Test]
        public void SkipWhiteSpace_AllWhitespaceLocations()
        {
            Assert.AreEqual(2, ("  2 first non whitespace charater").SkipWhiteSpace(0));
            Assert.AreEqual(2, ("012  5 next non whitespace charater").SkipWhiteSpace(2));
            Assert.AreEqual(5, ("012  5 next non whitespace charater").SkipWhiteSpace(3));
            const string TestEndString = "012  567   ";
            Assert.AreEqual(TestEndString.Length, TestEndString.SkipWhiteSpace(8));
            Assert.AreEqual(0, string.Empty.SkipWhiteSpace(0));
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SkipWhiteSpace_InvalidStart_ThrowsException()
        {
            int n = "123".SkipWhiteSpace(10);
            Assert.Fail("SkipWhiteSpace should have thrown an ArgumentOutOfRangeException");
        }

        [Test]
        public void RemoveCharsTest()
        {
            string[] inputs = { "asdf", "as,df", "a,s:df:", ":asdf:", "|a{s}d:f,", };
            char[] dividers = { ',', ':', '|', '{', '}', };
            foreach (string s in inputs)
            {
                Assert.AreEqual("asdf", s.RemoveChars(dividers));
            }

            const string inputString = "asdf";
            Assert.AreEqual(inputString, inputString.RemoveChars(new char[0]));


        }

        [Test]
        public void RemoveCharTest()
        {
            string[] inputs = { "asdf", "as,df", "a,s,df,", ",asdf,", ",a,s,d,f,", };
            const char Divider = ',';
            foreach (string s in inputs)
            {
                Assert.AreEqual("asdf", s.RemoveChar(Divider));
            }
        }
    }
}
