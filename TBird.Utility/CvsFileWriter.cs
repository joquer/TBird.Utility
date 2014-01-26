namespace TBird.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A utility class that parses CSV file and returns each line parsed as a set of tokens.
    /// </summary>
    public class CsvFileWriter : IDisposable
    {
        private readonly char lineSeparator;

        /// <summary>
        /// The <c>StreamWriter</c> used to write out the file data.
        /// </summary>
        private StreamWriter writer;

        /// <summary>
        /// Gets the current line in the file.
        /// </summary>
        public int LineNumber { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvFileWriter"/> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="separator">The separator.</param>
        public CsvFileWriter(StreamWriter writer, char separator)
            : this()
        {
            Contract.Requires(writer != null);
            Contract.Ensures(this.writer != null);
            this.writer = writer;
            this.lineSeparator = separator;
            this.LineNumber = 0;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="CsvFileWriter"/> class from being created.
        /// </summary>
        private CsvFileWriter()
        {
            this.LineNumber = 0;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="CsvFileWriter" /> class.
        /// </summary>
#if !(NETFX_CORE || WINDOWS_PHONE)
        [ExcludeFromCodeCoverage]
#endif
        ~CsvFileWriter()
        {
            // Finalizer calls Dispose(false)
            this.Dispose(false);
        }

        /// <summary>
        /// Closes the current stream and releases any resources (such as sockets and file handles) associated with the current stream.
        /// </summary>
        public void Close()
        {
            if (this.writer != null)
            {
#if !NETFX_CORE
                this.writer.Close();
#endif
                this.writer.Dispose();
                this.writer = null;     
            }
        }

        public void WriteHeaders(IEnumerable<string> headers)
        {
            Contract.Requires(headers != null);
            for (int i = 0; i < headers.Count(); i++)
            {
                this.writer.Write(headers.ElementAt(i));
                if (i < headers.Count() - 1)
                {
                    this.writer.Write(this.lineSeparator);
                }
            }

            this.writer.WriteLine(string.Empty);
        }

        public void WriteData(IEnumerable<string> columns)
        {
            Contract.Requires(columns != null);
            for (int i = 0; i < columns.Count(); i++)
            {
                this.writer.Write(columns.ElementAt(i));
                if (i < columns.Count() - 1)
                {
                    this.writer.Write(this.lineSeparator);
                }
            }

            this.writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// Calls Dispose(true);
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Close();
            }
        }
    }
}
