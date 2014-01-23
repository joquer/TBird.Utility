namespace TBird.Utility.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;

    public static class DatabaseVerifier
    {
        /// <summary>
        /// Verifies the enums against an ADO.Net data provider.
        /// Note:  This method is excluded from unit testing because it will try to verify all Enumerations.
        /// For the unit test not all the bindings are valid, so this would fail.  The main
        /// portion of this routine is the call to VerifyEnumeration which is tested separately.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <returns>A list of differences found between the Enumeration and the data source values.</returns>
        [ExcludeFromCodeCoverage]
        public static IEnumerable<string> VerifyEnums(DbConnection conn)
        {
            Contract.Requires(conn != null);
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);
            EnumerationVerifier ev = new EnumerationVerifier(new AdoNetValueProvider(conn));

            List<string> differences = new List<string>();

            foreach (Type t in Assembly.GetAssembly(ev.GetType()).GetTypes())
            {
                if (t.GetCustomAttributes(typeof(EnumerationTableBindingsAttribute), false)
                    .Cast<EnumerationTableBindingsAttribute>().FirstOrDefault() != null)
                {
                    differences.AddRange(ev.VerifyEnumeration(t));
                }
            }

            return differences;
        }
    }
}
