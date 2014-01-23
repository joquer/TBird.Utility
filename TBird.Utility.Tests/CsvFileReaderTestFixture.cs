namespace TBird.Utility.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using CPM.CSV.Tests;

    using NUnit.Framework;

    [TestFixture]
    public class CsvFileReaderTestFixture
    {
        private const char ColumnSeparator = ',';

        private static readonly CsvTestResult Headers = new CsvTestResult(
            "ColumnOne,ColumnTwo,ColumnThree,ColumnFour",
            new[] { "ColumnOne", "ColumnTwo", "ColumnThree", "ColumnFour" });

        private static readonly IEnumerable<CsvTestResult> TestResults = new[]
            {
                new CsvTestResult("One,Two,Three,Four", new[] { "One", "Two", "Three", "Four" }),
                new CsvTestResult("One,\"Two\",Three,Four", new[] { "One", "\"Two\"", "Three", "Four" }),
                new CsvTestResult("One,\"Two\"\"Two\",Three,Four", new[] { "One", "\"Two\"\"Two\"", "Three", "Four" }),
            };

        [Test]
        public void CsvFileRead_ReadFile_AllResultsMatch()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    foreach (CsvTestResult line in TestResults)
                    {
                        writer.WriteLine(line.TextLine);
                    }

                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        using (CsvFileReader csvFileReader = new CsvFileReader(reader, ColumnSeparator))
                        {
                            Assert.IsFalse(csvFileReader.HasHeaders);
                            Assert.AreEqual(0, csvFileReader.LineNumber);
                            foreach (CsvTestResult test in TestResults)
                            {
                                IEnumerable<string> tokens = csvFileReader.NextLine();
                                Assert.AreEqual(TestResults.ElementAt(0).Columns.Count(), csvFileReader.NumberColumns);
                                Assert.IsNotNull(tokens);
                                CollectionAssert.AreEqual(test.Columns, tokens);
                            }
                        }
                    }
                }
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CsvFileRead_ReadFileWithBlankHeader_ThrowsInvalidOperation()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(string.Empty);
                    writer.WriteLine(string.Empty);
                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        using (CsvFileReader csvFileReader = new CsvFileReader(reader, ColumnSeparator, true))
                        {
                            IEnumerable<string> tokens;
                            while ((tokens = csvFileReader.NextLine()) != null)
                            {
                                Assert.IsNotNull(tokens);
                            }
                        }
                    }
                }
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CsvFileRead_ReadInvalidData_ThrowsInvalidOperation()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine("Column1,Column2,Columns3");
                    writer.WriteLine("1,2,3");
                    writer.WriteLine("1,2,3,4");
                    writer.WriteLine("1,2,3");
                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        using (CsvFileReader csvFileReader = new CsvFileReader(reader, ColumnSeparator, true))
                        {
                            IEnumerable<string> tokens;
                            while ((tokens = csvFileReader.NextLine()) != null)
                            {
                                Assert.IsNotNull(tokens);
                            }
                        }
                    }
                }
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Lines_MethodChainReturn_ReturnsAllLines()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(Headers.Columns);
                    foreach (CsvTestResult line in TestResults)
                    {
                        writer.WriteLine(line.TextLine);
                    }

                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        using (CsvFileReader csvFileReader = new CsvFileReader(reader, ColumnSeparator, true))
                        {
                            int count = 0;
                            foreach (IEnumerable<string> tokens in csvFileReader.Lines)
                            {
                                Assert.IsNotNull(tokens);
                                CollectionAssert.AreEqual(TestResults.ElementAt(count).Columns, tokens);
                                count++;
                            }
                            Assert.AreEqual(TestResults.Count(), count);
                        }
                    }
                }
            }
        }
    }
}
