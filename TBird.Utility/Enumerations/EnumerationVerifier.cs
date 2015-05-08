// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerationVerifier.cs" company="Advisory Board Company - Crimson">
//   Copyright © 2014 Advisory Board Company - Crimson
// </copyright>
// <summary>
//   Defines the EnumerationVerifier type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility.Enumerations
{
    using System;
    using System.Collections.Generic;
#if !NETFX_CORE
    using System.Diagnostics.CodeAnalysis;
#endif
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    public class EnumerationVerifier
    {
        private readonly IEnumerationValueProvider provider;

        public EnumerationVerifier(IEnumerationValueProvider valueProvider)
        {
            Contract.Requires(valueProvider != null);
            Contract.Ensures(this.provider != null);
            Contract.Ensures(this.provider == valueProvider);
            this.provider = valueProvider;
        }

#if !NETFX_CORE
        [ExcludeFromCodeCoverage]
#endif
        public static IEnumerable<string> ValidateFromAssemblyOf<T>(IEnumerationValueProvider provider)
        {
            Contract.Requires(provider != null);
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);
            return VerifyAssembly(provider, typeof(T).Assembly);
        }

#if !NETFX_CORE
        [ExcludeFromCodeCoverage]
#endif
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Validating the parameters with Code Contracts")]
        public static IEnumerable<string> ValidateFromAssemblies(IEnumerationValueProvider provider, Assembly[] assemblies)
        {
            Contract.Requires(provider != null);
            Contract.Requires(assemblies != null);
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);
            List<string> differences = new List<string>();
            foreach (Assembly assem in assemblies)
            {
                differences.AddRange(VerifyAssembly(provider, assem));
            }

            return differences;
        }

#if !NETFX_CORE
        [ExcludeFromCodeCoverage]
#endif
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Validating the parameters with Code Contracts")]
        public static IEnumerable<string> VerifyAssembly(IEnumerationValueProvider provider, Assembly assembly)
        {
            Contract.Requires(provider != null);
            Contract.Requires(assembly != null);
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);
            EnumerationVerifier ev = new EnumerationVerifier(provider);

            List<string> differences = new List<string>();

            foreach (Type t in assembly.GetTypes())
            {
                if (t.GetCustomAttributes(typeof(EnumerationTableBindingsAttribute), false)
                    .Cast<EnumerationTableBindingsAttribute>().FirstOrDefault() != null)
                {
                    differences.AddRange(ev.VerifyEnumeration(t));
                }
            }

            return differences;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Validating the parameters with Code Contracts")]
        public IEnumerable<string> VerifyEnumeration(Type enumType)
        {
            Contract.Requires(enumType != null);
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);
            const string GetValuesName = "GetValues";
            List<string> differences = new List<string>();

            EnumerationTableBindingsAttribute bindings = EnumerationTableBindingsAttribute.GetEnumBindings(enumType);
            if (bindings == null)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "There are no table bindings for Type {0}", enumType.FullName));
            }

            MethodInfo method = ReflectionHelper.FindMethod(enumType, GetValuesName);
            if (method == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Method {0} not found on type {1}",
                        GetValuesName,
                        enumType.FullName));
            }
            else
            {
                IEnumerable<EnumerationValue> values =
                    method.Invoke(null, new object[0]) as IEnumerable<EnumerationValue>;
                if (values == null)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.InvariantCulture, "{0}:  No enuemration values found on type", enumType.FullName));
                }

                List<EnumerationValue> codeValues = values.ToList();
                List<EnumerationValue> databaseValues = this.provider.GetValues(bindings).ToList();
                if (databaseValues.Count() != codeValues.Count())
                {
                    differences.Add(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "{0}: Number of elements mismatch: Enumeration has {1} elements and Database has {2}",
                            enumType.FullName,
                            codeValues.Count(),
                            databaseValues.Count()));
                }
                else
                {
                    for (int i = 0; i < databaseValues.Count(); i++)
                    {
                        if (!databaseValues.ElementAt(i).Equals(codeValues.ElementAt(i)))
                        {
                            differences.Add(
                                string.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0}.{1} does not match - C#: {2} - Database {3}",
                                    enumType.Name,
                                    codeValues.ElementAt(i).Name,
                                    codeValues.ElementAt(i),
                                    databaseValues.ElementAt(i)));
                        }
                    }
                }
            }

            return differences;
        }
    }
}
