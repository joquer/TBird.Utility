namespace TBird.Utility.Tests
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Defines TestUtils class.
    /// </summary>
    public static class TestUtils
    {
        /// <summary>
        /// Flattens the Collection of messages into a single string with newlines to seperate each message.
        /// </summary>
        /// <param name="messages">A collection of single line messages.</param>
        /// <returns>The messages as a single string</returns>
        public static string FlattenMessages(IEnumerable<string> messages)
        {
            if (messages == null)
            {
                return string.Empty;
            }

            StringBuilder bldr = new StringBuilder();
            foreach (string msg in messages)
            {
                bldr.Append("  ");
                bldr.AppendLine(msg);
            }

            return bldr.ToString();
        }
    }
}
