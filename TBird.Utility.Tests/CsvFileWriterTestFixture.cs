namespace CPM.CSV.Tests
{
    using System.Collections.Generic;
    using System.IO;

    using NUnit.Framework;

    using TBird.Utility;
    using TBird.Utility.Tests;

    [TestFixture]
    public class CsvFileWriterTestFixture
    {
        private const char ColumnSeparator = '|';

        private static readonly CsvTestResult Headers = new CsvTestResult(
            "ColumnOne, ColumnTwo, ColumnThree, ColumnFour",
            new[] { "ColumnOne", "ColumnTwo", "ColumnThree", "ColumnFour" });

        private static readonly CsvTestResult[] TestResults = new[]
                                                                   {
                                                                       new CsvTestResult(
                                                                           "1|2|3|4",
                                                                           new[] { "1", "2", "3", "4" }),
                                                                       new CsvTestResult(
                                                                           "4|3|2|1",
                                                                           new[] { "4", "3", "2", "1" }),
                                                                       new CsvTestResult(
                                                                           "5|6|7|8",
                                                                           new[] { "5", "6", "7", "8" }),
                                                                       new CsvTestResult(
                                                                           "8|7|6|5",
                                                                           new[] { "8", "7", "6", "5" }),
                                                                   };


        [Test]
        public void CsvWriteFile_WriteAllData_ValidData()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    using (CsvFileWriter csvFileWriter = new CsvFileWriter(writer, ColumnSeparator))
                    {
                        foreach (CsvTestResult test in TestResults)
                        {
                            csvFileWriter.WriteData(test.Columns);
                        }

                        writer.Flush();
                        stream.Seek(0, SeekOrigin.Begin);

                        using (StreamReader reader = new StreamReader(stream))
                        {
                            foreach (CsvTestResult line in TestResults)
                            {
                                Assert.AreEqual(line.TextLine, reader.ReadLine());
                            }
                            Assert.IsNull(reader.ReadLine());
                        }
                    }
                }
            }
        }

        [Test]
        public void CsvWriteFileWithHeaders_WriteAllData_ValidData()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    using (CsvFileWriter csvFileWriter = new CsvFileWriter(writer, ColumnSeparator))
                    {
                        writer.WriteLine(Headers.TextLine);
                        foreach (CsvTestResult test in TestResults)
                        {
                            csvFileWriter.WriteData(test.Columns);
                        }

                        writer.Flush();
                        stream.Seek(0, SeekOrigin.Begin);

                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string headerLine = reader.ReadLine();
                            Assert.AreEqual(Headers.TextLine, headerLine);
                            foreach (CsvTestResult line in TestResults)
                            {
                                Assert.AreEqual(line.TextLine, reader.ReadLine());
                            }
                            Assert.IsNull(reader.ReadLine());
                        }
                    }
                }
            }
        }

        [
            Test]
        public void CsvWriteRead_WriteReadData_DataMatches()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    using (CsvFileWriter csvFileWriter = new CsvFileWriter(writer, ColumnSeparator))
                    {
                        foreach (CsvTestResult test in TestResults)
                        {
                            csvFileWriter.WriteData(test.Columns);
                        }

                        writer.Flush();
                        stream.Seek(0, SeekOrigin.Begin);

                        using (StreamReader reader = new StreamReader(stream))
                        {
                            using (CsvFileReader csvFileReader = new CsvFileReader(reader, ColumnSeparator))
                            {
                                foreach (CsvTestResult test in TestResults)
                                {
                                    IEnumerable<string> tokens = csvFileReader.NextLine();
                                    Assert.IsNotNull(tokens);
                                    CollectionAssert.AreEqual(test.Columns, tokens);
                                }
                            }
                        }
                    }
                }
            }
        }

        [Test]
        public void CsvWriteReadWithHeaders_WriteReadData_DataMatches()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    using (CsvFileWriter csvFileWriter = new CsvFileWriter(writer, ColumnSeparator))
                    {
                        csvFileWriter.WriteHeaders(Headers.Columns);
                        foreach (CsvTestResult test in TestResults)
                        {
                            csvFileWriter.WriteData(test.Columns);
                        }

                        writer.Flush();
                        stream.Seek(0, SeekOrigin.Begin);

                        using (StreamReader reader = new StreamReader(stream))
                        {
                            using (CsvFileReader csvFileReader = new CsvFileReader(reader, ColumnSeparator, true))
                            {
                                CollectionAssert.AreEqual(Headers.Columns, csvFileReader.Headers);
                                foreach (CsvTestResult test in TestResults)
                                {
                                    IEnumerable<string> tokens = csvFileReader.NextLine();
                                    Assert.IsNotNull(tokens);
                                    CollectionAssert.AreEqual(test.Columns, tokens);
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
