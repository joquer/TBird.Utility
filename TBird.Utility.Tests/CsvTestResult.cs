namespace TBird.Utility.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    public class CsvTestResult
    {
        private IEnumerable<string> columns = Enumerable.Empty<string>();

        public string TextLine { get; private set; }

        public IEnumerable<string> Columns
        {
            get
            {
                return this.columns;
            }

            private set
            {
                this.columns = value ?? Enumerable.Empty<string>();
            }
        }

        public CsvTestResult(string input, IEnumerable<string> columns)
        {
            this.TextLine = input;
            this.Columns = columns;
        }
    }
}