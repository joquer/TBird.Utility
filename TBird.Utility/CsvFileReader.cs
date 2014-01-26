// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Advisory Board Company - Crimson" file="CsvFileReader.cs">
//   Copyright © 2013 - Advisory Board Company - Crimson
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
    public class CsvFileReader : IDisposable
    {
        private readonly char fieldSeparator;

        /// <summary>
        /// The <c>StreamReader</c> used to read in the file data.
        /// </summary>
        private StreamReader reader;

        /// <summary>
        /// Gets a value indicating whether the first line of the CSV File has headers.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the first line of the file contains column headers; otherwise, <c>false</c>.
        /// </value>
        public bool HasHeaders { get; private set; }

        /// <summary>
        /// The list of column headers from the CSV File.
        /// </summary>
        /// <value>
        /// The list of column names from the CSV file header, or if <see cref="HasHeaders"/> is <code>false</code>
        /// then an empty list is returned.
        /// </value>
        public IEnumerable<string> Headers { get; private set; } 

        /// <summary>
        /// Gets the current line in the file.
        /// </summary>
        public int LineNumber { get; private set; }

        /// <summary>
        /// Gets the number columns in the file.  This is set when the first line of the file is read, and if
        /// any lines do not have this number of columns, an exception is thrown.
        /// </summary>
        /// <value>
        /// The number columns.
        /// </value>
        public int NumberColumns { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvFileReader"/> class. 
        /// </summary>
        /// <param name="reader">The <c>StreamReader</c> for the csv file to parse.</param>
        /// <param name="separator">The column separator.</param>
        /// <param name="hasHeaders">Should the first line of the file be read as a header.</param>
        /// <exception cref="ArgumentException">
        /// path is a zero-length string, contains only white space, or contains one or more invalid characters as defined by InvalidPathChars.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// path is null.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// The specified path is invalid, (for example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="IOException">
        /// An I/O error occurred while opening the file.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// path specified a file that is read-only.
        /// -or- 
        /// This operation is not supported on the current platform.
        /// -or- 
        /// path specified a directory.
        /// -or- 
        /// The caller does not have the required permission. 
        /// -or-
        /// mode is Create and the specified file is a hidden file.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The file specified in path was not found.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// path is an invalid format.
        /// </exception>
        public CsvFileReader(StreamReader reader, char separator, bool hasHeaders = false)
            : this()
        {
            Contract.Requires(reader != null);
            Contract.Ensures(this.reader != null);
            this.reader = reader;
            this.fieldSeparator = separator;
            this.HasHeaders = hasHeaders;
            this.Headers = Enumerable.Empty<string>();
            if (this.HasHeaders)
            {
                string firstLine = reader.ReadLine();
                if (string.IsNullOrEmpty(firstLine))
                {
                    throw new InvalidOperationException("Missing Header");
                }

                this.Headers = firstLine[0] == '#'
                                   ? firstLine.Substring(1).Split(this.fieldSeparator)
                                   : firstLine.Split(this.fieldSeparator);
                this.NumberColumns = this.Headers.Count();
                this.LineNumber++;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvFileReader"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="separator">The separator.</param>
        public CsvFileReader(StreamReader reader, char separator)
            : this(reader, separator, false)
        {
            Contract.Requires(reader != null);
            Contract.Ensures(this.reader != null);
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="CsvFileReader"/> class from being created.
        /// </summary>
        private CsvFileReader()
        {
            this.LineNumber = 0;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="CsvFileReader"/> class. 
        /// </summary>
#if !(NETFX_CORE || WINDOWS_PHONE)
        [ExcludeFromCodeCoverage]
#endif
        ~CsvFileReader()
        {
            // Finalizer calls Dispose(false)
            this.Dispose(false);
        }

        /// <summary>
        /// Closes the current stream and releases any resources (such as sockets and file handles) associated with the current stream.
        /// </summary>
        public void Close()
        {
            if (this.reader != null)
            {
#if !NETFX_CORE
                this.reader.Close();
#endif
                this.reader.Dispose();
                this.reader = null;     
            }
        }

        /// <summary>
        /// Returns the next line of the CSV file parsed as a Collection of strings.
        /// </summary>
        /// <returns>
        /// A Collection of strings of the CSV values from the next line in the file.
        /// </returns>
        public IEnumerable<string> NextLine()
        {
            string line = this.reader.ReadLine();
            if (line == null)
            {
                return null;
            }
            if (line.Length == 0)
            {
                return Enumerable.Empty<string>();
            }
            this.LineNumber++;

            IEnumerable<string> columns = line.Split(new[] { this.fieldSeparator });
            if (this.NumberColumns == 0)
            {
                this.NumberColumns = columns.Count();
            }
            else if (columns.Count() != this.NumberColumns)
            {
                throw new InvalidOperationException(
                    string.Format("Line {0} has {1} columns, but expected {2}", this.LineNumber, columns.Count(), this.NumberColumns));
            }
            return columns;
        }

        public IEnumerable<IEnumerable<string>> Lines
        {
            get
            {
                IEnumerable<string> tokens;
                while ((tokens = this.NextLine()) != null)
                {
                    yield return tokens;
                }
            }
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
