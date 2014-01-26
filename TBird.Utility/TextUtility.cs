// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextUtility.cs" company="Mr Smarti Pantz LLC">
//     Copyright © Mr. Smarti Pantz LLC 2011, All rights reserved.
// </copyright>
// <summary>
//   Defines the TextUtility type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#if !SILVERLIGHT
#define NOT_SILVERLIGHT
#endif

namespace TBird.Utility
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A collection of utility routines for strings and general text.
    /// </summary>
    public static class TextUtility
    {
        /// <summary>
        /// Determines if a char is a member of a list of characters.
        /// </summary>
        /// <param name="list">
        /// The list of characters to be checked.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// <code>true</code> if item is a member of the list, otherwise <code>false</code>
        /// </returns>
        public static bool IsMemberOf(IEnumerable<char> list, char item)
        {
            return list != null && list.Contains(item);
        }

        /// <summary>
        /// Removes the quotes from a list of string.  Each string in a list of string
        /// is scanned for ".  If it contains a quote, it is removed and a new list of
        /// strings is returned.
        /// </summary>
        /// <param name="tokens">A new list of strings with all "'s removed.</param>
        /// <returns></returns>
        public static IEnumerable<string> RemoveQuotes(IEnumerable<string> tokens)
        {
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);
            if (tokens == null)
            {
                return Enumerable.Empty<string>();
            }

            Collection<string> newTokens = new Collection<string>();
            foreach (string token in tokens)
            {
                if (string.IsNullOrEmpty(token))
                {
                    newTokens.Add(string.Empty);
                }
                else
                {
                    StringBuilder bldr = new StringBuilder();
                    foreach (char c in token)
                    {
                        if (c != '\"')
                        {
                            bldr.Append(c);
                        }
                    }

                    newTokens.Add(bldr.ToString());
                }
            }

            return newTokens;
        }

        public static int ParseInt32(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            string digitsOnly = value.RemoveChars(new char[] { ',' });
            int result;
            return int.TryParse(digitsOnly, out result) ? result : 0;
        }

        public static long ParseInt64(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            string digitsOnly = value.RemoveChars(new char[] { ',' });
            if (string.IsNullOrEmpty(digitsOnly))
            {
                return 0;
            }

            if (digitsOnly[digitsOnly.Length - 1] == 'B')
            {
                long longValue;
                if (long.TryParse(digitsOnly.Substring(0, digitsOnly.Length - 1), out longValue))
                {
                    return longValue * 1000 * 1000 * 1000;
                }

                return 0;
            }

            if (value[value.Length - 1] == 'M')
            {
                long longValue;
                if (long.TryParse(digitsOnly.Substring(0, digitsOnly.Length - 1), out longValue))
                {
                    return (long)longValue * 1000 * 1000;
                }
                return 0;
            }

            long result;
            if (long.TryParse(digitsOnly, out result))
            {
                return result;
            }

            return 0;
        }

        public static double ParseDouble(string value)
        {
            string noPercentSign = value.RemoveChars(new char[] { '%' });
            double result;
            if (double.TryParse(noPercentSign, out result))
            {
                return result;
            }

            return 0;
        }

        public static decimal ParseDecimal(string value)
        {
            string noPercentSign = value.RemoveChars(new char[] { '%' });
            decimal result;
            if (decimal.TryParse(noPercentSign, out result))
            {
                return result;
            }

            return 0;
        }

        public static string CreateIndentString(int indent)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            StringBuilder bldr = new StringBuilder();
            for (int n = 0; n < indent; n++)
            {
                bldr.Append(' ');
            }

            return bldr.ToString();
        }
    }
}
