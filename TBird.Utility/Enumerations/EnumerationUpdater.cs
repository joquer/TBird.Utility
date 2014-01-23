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
    using System.Text;

    public class EnumerationUpdater
    {
        private readonly IEnumerationUpdateProvider updater;

        private enum Operation
        {
            Insert,

            Update,
        }

        private class UpdaterAction
        {
            public Operation UpdateOperation { get; set; }

            public EnumerationValue CodeValue { get; set; }
        }

        public EnumerationUpdater(IEnumerationUpdateProvider updateProvider)
        {
            Contract.Requires(updateProvider != null);
            Contract.Ensures(this.updater != null);
            Contract.Ensures(this.updater == updateProvider);
            this.updater = updateProvider;
        }

#if !NETFX_CORE
        [ExcludeFromCodeCoverage]
#endif
        public static UpdateStats UpdateFromAssemblyOf<T>(IEnumerationUpdateProvider provider)
        {
            Contract.Requires(provider != null);
            Contract.Ensures(Contract.Result<UpdateStats>() != null);
            return UpdateAssembly(provider, typeof(T).Assembly);
        }

#if !NETFX_CORE
        [ExcludeFromCodeCoverage]
#endif
        public static void ClearFromAssemblyOf<T>(IEnumerationUpdateProvider provider)
        {
            Contract.Requires(provider != null);
            ClearAssembly(provider, typeof(T).Assembly);
        }

#if !NETFX_CORE
        [ExcludeFromCodeCoverage]
#endif
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Code Contracts validate arguments")]
        public static UpdateStats UpdateFromAssemblies(IEnumerationUpdateProvider provider, Assembly[] assemblies)
        {
            Contract.Requires(provider != null);
            Contract.Requires(assemblies != null);
            Contract.Ensures(Contract.Result<UpdateStats>() != null);
            UpdateStats results = new UpdateStats();
            foreach (Assembly assem in assemblies)
            {
                UpdateStats stats = UpdateAssembly(provider, assem);
                results.UpdateCount += stats.UpdateCount;
                results.AddCount += stats.AddCount;
            }

            return results;
        }

#if !NETFX_CORE
        [ExcludeFromCodeCoverage]
#endif
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Code Contracts validate arguments")]
        public static void ClearFromAssemblies(IEnumerationUpdateProvider provider, Assembly[] assemblies)
        {
            Contract.Requires(provider != null);
            Contract.Requires(assemblies != null);
            foreach (Assembly assem in assemblies)
            {
                ClearAssembly(provider, assem);
            }
        }

#if !NETFX_CORE
        [ExcludeFromCodeCoverage]
#endif
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Code Contracts validate arguments")]
        public static UpdateStats UpdateAssembly(IEnumerationUpdateProvider provider, Assembly assembly)
        {
            Contract.Requires(provider != null);
            Contract.Requires(assembly != null);
            Contract.Ensures(Contract.Result<UpdateStats>() != null);
            EnumerationUpdater eu = new EnumerationUpdater(provider);

            UpdateStats results = new UpdateStats();
            foreach (Type t in assembly.GetTypes())
            {
                if (t.GetCustomAttributes(typeof(EnumerationTableBindingsAttribute), false)
                    .Cast<EnumerationTableBindingsAttribute>().FirstOrDefault() != null)
                {
                    UpdateStats stats = eu.Update(t);
                    results.UpdateCount += stats.UpdateCount;
                    results.AddCount += stats.AddCount;
                }
            }

            return results;
        }

#if !NETFX_CORE
        [ExcludeFromCodeCoverage]
#endif
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Code Contracts validate arguments")]
        public static void ClearAssembly(IEnumerationUpdateProvider provider, Assembly assembly)
        {
            Contract.Requires(provider != null);
            Contract.Requires(assembly != null);
            EnumerationUpdater eu = new EnumerationUpdater(provider);

            foreach (Type t in assembly.GetTypes())
            {
                if (t.GetCustomAttributes(typeof(EnumerationTableBindingsAttribute), false)
                    .Cast<EnumerationTableBindingsAttribute>().FirstOrDefault() != null)
                {
                    eu.Clear(t);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", Justification = "GetValues is the name of a method and needs to be displayed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Code Contracts validate arguments")]
        public UpdateStats Update(Type enumType)
        {
            Contract.Requires(enumType != null);
            Contract.Ensures(Contract.Result<UpdateStats>() != null);
            const string GetValuesName = "GetValues";

            EnumerationTableBindingsAttribute bindings = EnumerationTableBindingsAttribute.GetEnumBindings(enumType);
            if (bindings == null)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "There are no table bindings for Type {0}", enumType.FullName));
            }

            MethodInfo method = ReflectionHelper.FindMethod(enumType, GetValuesName);
            if (method == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Method {0} not found on type {1}", GetValuesName, enumType.FullName));
            }

            List<EnumerationValue> databaseValues = this.updater.GetValues(bindings).ToList();
            IEnumerable<EnumerationValue> values = method.Invoke(null, new object[0]) as IEnumerable<EnumerationValue>;
            if (values == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "{0} method returns null for type {1}", GetValuesName, enumType.FullName));
            }

            UpdateStats stats = new UpdateStats();
            List<UpdaterAction> actions = new List<UpdaterAction>();
            foreach (EnumerationValue value in values)
            {
                EnumerationValue databaseValue = databaseValues.FirstOrDefault(x => x.Value == value.Value);
                if (databaseValue == null)
                {
                    actions.Add(new UpdaterAction() { UpdateOperation = Operation.Insert, CodeValue = value, });
                    stats.AddCount++;
                }
                else
                {
                    databaseValues.Remove(databaseValue);
                    if (databaseValue.Name != value.Name || databaseValue.DisplayName != value.DisplayName)
                    {
                        actions.Add(new UpdaterAction() { UpdateOperation = Operation.Update, CodeValue = value, });
                        stats.UpdateCount++;
                    }
                }
            }

            if (databaseValues.Any())
            {
                StringBuilder bldr = new StringBuilder();
                bldr.AppendFormat("Extra values exist in Database for {0}:", enumType.FullName);
                foreach (EnumerationValue v in databaseValues)
                {
                    if (v != null)
                    {
                        bldr.AppendFormat(" {0}({1})", v.Name, v.Value);
                    }
                }

                throw new InvalidOperationException(bldr.ToString());
            }

            foreach (UpdaterAction action in actions)
            {
                if (action != null)
                {
                    if (action.UpdateOperation == Operation.Insert)
                    {
                        this.updater.InsertValue(bindings, action.CodeValue);
                    }
                    else
                    {
                        this.updater.UpdateValue(bindings, action.CodeValue);
                    }
                }
            }

            return stats;
        }

        public void Clear(Type enumType)
        {
            Contract.Requires(enumType != null);

            EnumerationTableBindingsAttribute bindings = EnumerationTableBindingsAttribute.GetEnumBindings(enumType);
            if (bindings == null)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "There are no table bindings for Type {0}", enumType.FullName));
            }

            this.updater.Clear(bindings);
        }
    }
}
