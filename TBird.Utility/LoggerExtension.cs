namespace TBird.Utility
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Resources;

    //using log4net;

    public static class LoggerExtensions
    {
        //private static readonly ResourceManager StringManager = new ResourceManager(
        //    "TBird.Utility.Strings", Assembly.GetExecutingAssembly());

        //public static string GetResourceString(string name)
        //{
        //    if (string.IsNullOrEmpty(name))
        //        return string.Empty;
        //    return StringManager.GetString(name, CultureInfo.CurrentUICulture);
        //}

        //public static void LogErrorException(this Logger logger, Exception ex)
        //{
        //    if (ex == null)
        //        return;
        //    if (logger == null) return;
        //    logger.Error(
        //        CultureInfo.CurrentCulture,
        //        StringManager.GetString("CaughtException", CultureInfo.CurrentUICulture),
        //        ex.GetType().Name);
        //    logger.Error(StringManager.GetString("ExceptionDetails", CultureInfo.CurrentUICulture));
        //    for (Exception iex = ex; iex != null; iex = iex.InnerException)
        //    {
        //        logger.Error(
        //            CultureInfo.CurrentCulture,
        //            string.Format(CultureInfo.CurrentCulture, "{0}: {1}", iex.GetType().Name, iex.Message));
        //    }

        //    if (ex.StackTrace != null)
        //    {
        //        logger.Error(StringManager.GetString("StackTrace", CultureInfo.CurrentUICulture));

        //        int start = 0;
        //        while (start < ex.StackTrace.Length)
        //        {
        //            int n = ex.StackTrace.IndexOf(Environment.NewLine, start, StringComparison.CurrentCulture);
        //            if (n <= 0)
        //            {
        //                logger.Error("  " + ex.StackTrace.Substring(start));
        //                start = ex.StackTrace.Length;
        //            }
        //            else
        //            {
        //                logger.Error("  " + ex.StackTrace.Substring(start, n - start));
        //                start += n + Environment.NewLine.Length;
        //            }
        //        }
        //    }
        //}

        //public static void LogErrorException(this Logger logger, string msg, Exception ex)
        //{
        //    if (logger == null)
        //        return;
        //    logger.ErrorException(msg, ex);
        //    LogErrorException(logger, ex);
        //}
    }
}
