using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TBird.Utility
{
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Text;

    using log4net;

    public static class LoggerUtil
    {
        public static void LogException(ILog logger, Exception ex)
        {
            Contract.Requires(logger != null);
            if (ex == null)
            {
                return;
            }

            string indent = string.Empty;
            for (Exception iex = ex; iex != null; iex = iex.InnerException)
            {
                logger.Error(
                    string.Format(CultureInfo.CurrentCulture, "{0}{1}: {2}", indent, iex.GetType().Name, iex.Message));
                indent = "  ";
            }

            if (ex.StackTrace != null)
            {
                logger.Error("StackTrace...");

                using (StringReader reader = new StringReader(ex.StackTrace))
                {
                    for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                    {
                        logger.Error(string.Format("  {0}", line));
                    }
                }
            }
        }
    }
}