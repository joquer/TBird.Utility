// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Mr Smarti Pantz LLC">
//   Copyright 2011 Mr Smarti Pantz LLC
// </copyright>
// <summary>
//   Defines the StringExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    
    /// <summary>
    /// A set of extension methods for the string class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Skips over whitespace starting at the start position and returns the index past the whitespace.
        /// </summary>
        /// <param name="text">The text to be searched.</param>
        /// <param name="start">The start index from which whitespace is to be skipped.</param>
        /// <returns>The index after the whitespace or string.Length if the end of the string is reached.</returns>
        public static int SkipWhiteSpace(this string text, int start)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            if (start < 0 || start > text.Length)
            {
#if SILVERLIGHT
                throw new ArgumentOutOfRangeException(
                    "start",
                    string.Format("the value of start, {0}, must be between 0 and the the length of the string", start));
#else
                throw new ArgumentOutOfRangeException(
                    "start", start, "value must be between 0 and the the length of the string");
#endif
            }

            int n = start;
            for (; n < text.Length && char.IsWhiteSpace(text[n]); n++)
            {
            }

            return n;
        }

        /// <summary>
        /// Determines whether the specified text is numeric.  I a string is numeric if it contains
        /// only numbers.  This routine does not check for commas, decimal points etc.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        ///   <c>true</c> if the specified text is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(this string text)
        {
            return text.Cast<char>().All(char.IsNumber);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "The parametesr are validated by Code Contracts.")]
        public static string RemoveChars(this string line, char[] dividers)
        {
            Contract.Requires(dividers != null);
            return dividers.Length == 0 ? line : line.Cast<char>().Where(ch => !TextUtility.IsMemberOf(dividers, ch)).ToString();
        }

        public static string RemoveChar(this string line, char divider)
        {
            return !line.Contains(divider.ToString()) ? line : RemoveChars(line, new char[] { divider });
        }

        /// <summary>
        /// Counts the number of lines in the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static int CountLines(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            int count = 0;
            int start = 0;
            bool done = false;
            while (done == false)
            {
                int n = text.IndexOf(Environment.NewLine, start, StringComparison.CurrentCulture);
                if (n == -1)
                {
                    done = true;
                    if (start < text.Length)
                    {
                        count++;
                    }
                }
                else
                {
                    start = n + Environment.NewLine.Length;
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Removes blank lines from the beginning of the string..
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="numberLines">The number lines.</param>
        /// <returns>A new string with any blank lines at the beginning removed.</returns>
        public static string RemoveLinesFromBeginning(this string text, int numberLines)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.CountLines() < numberLines)
            {
                return string.Empty;
            }

            int start = 0;
            for (int i = 0; i < numberLines; i++)
            {
                start = text.IndexOf(Environment.NewLine, start, StringComparison.CurrentCulture);
                if (start < 0)
                {
                    return string.Empty;
                }

                start += Environment.NewLine.Length;
            }

            if (start < text.Length)
            {
                return text.Substring(start);
            }

            return string.Empty;
        }

        /// <summary>
        /// Truncates the specified text to the spcified length, or leaves the string intact
        /// if the maxLength is greater than the length of the string.
        /// </summary>
        /// <param name="text">The text to be truncated.</param>
        /// <param name="maxLength">The new length of the string.</param>
        /// <returns>A new string truncated to the maxLength, or the same string if the maxLength
        /// is greater than the length of the string.</returns>
        public static string Truncate(this string text, int maxLength)
        {
            return string.IsNullOrEmpty(text)
                       ? string.Empty
                       : (text.Length <= maxLength ? text : text.Substring(0, maxLength));
        }
    }
}
