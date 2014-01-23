namespace TBird.Utility.Test
{
    using System.Collections.Generic;

    public class TestResult
    {
        public string TestString { get; private set; }
        public List<string> Results { get; private set; }

        public TestResult(string input, string[] results)
        {
            TestString = input;
            Results = new List<string>();
            Results.AddRange(results);
        }

        private static TestResult[] testResults =
        {
            new TestResult("", new string[] { string.Empty }),
            new TestResult("One, Two, Three, Four", new string[] { "One", "Two", "Three", "Four"}),
            new TestResult("One, \"Two\", Three, Four", new string[] { "One", "Two", "Three", "Four"}),
            new TestResult("One, \"Two\"\"Two\", Three, Four", new string[] { "One", "Two\"Two", "Three", "Four"}),
        };

        public static TestResult[] LineResults { get { return testResults; } }
    }

}
