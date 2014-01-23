namespace TBird.Utility.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Design.PluralizationServices;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Ties an Enumeration class into a database table so that the values can be verified against what
    /// is in the database.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class EnumerationTableBindingsAttribute : Attribute
    {
        private Dictionary<string, string> propertyBindings = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerationTableBindingsAttribute"/> class.  This attribute
        /// is used to associate an Enumeration class with a database table.  Each table must have three
        /// columns and the Value, Name and DisplayName are mapped to the columns using the strings
        /// <c>KeyColumn</c>, <c>NameColumn</c> and <c>DisplayNameColumn</c> properties.  The default
        /// behavior is to append Key to the TableName to get the Key Column and the other two columns
        /// are Name and Description.
        /// </summary>
        /// <param name="schemaName">THe schema name of the table.</param>
        /// <param name="tableName">Name of the table.</param>
        public EnumerationTableBindingsAttribute(string schemaName, string tableName)
        {
            Contract.Requires(!string.IsNullOrEmpty(schemaName));
            Contract.Requires(!string.IsNullOrEmpty(tableName));
            Contract.Ensures(this.propertyBindings != null);
            this.SchemaName = schemaName;
            this.TableName = tableName;
            PluralizationService ps = Pluralizer.GetService();

            this.KeyColumn = (ps.IsPlural(tableName) ? ps.Singularize(tableName) : tableName) + "Key";
            this.NameColumn = "Name";
            this.DisplayNameColumn = "Description";
        }

        public string SchemaName { get; private set; }

        public string TableName { get; private set; }

        public string KeyColumn { get; set; }

        public string NameColumn { get; set; }

        public string DisplayNameColumn { get; set; }

        public Dictionary<string, string> PropertyBindings
        {
            get
            {
                return this.propertyBindings;
            }
        }

        public static EnumerationTableBindingsAttribute GetEnumBindings(Type enumType)
        {
            Contract.Requires(enumType != null);
            EnumerationTableBindingsAttribute bindings =
                enumType.GetCustomAttributes(typeof(EnumerationTableBindingsAttribute), false)
                .Cast<EnumerationTableBindingsAttribute>().FirstOrDefault();
            if (bindings == null)
            {
                return null;
            }

#if NETFX_CORE
            foreach (PropertyInfo propertyInfo in enumType.GetTypeInfo().DeclaredProperties.Where(x => x.GetCustomAttributes<EnumerationPropertyBindingAttribute>().Any()))
#else
            foreach (
                PropertyInfo propertyInfo in
                    enumType.GetProperties().Where(
                        x => x.GetCustomAttributes(typeof(EnumerationPropertyBindingAttribute), true).Any()))
#endif
            {
                EnumerationPropertyBindingAttribute propertyBinding =
                    propertyInfo.GetCustomAttributes(typeof(EnumerationPropertyBindingAttribute), true)
                    .Cast<EnumerationPropertyBindingAttribute>().FirstOrDefault();
                if (propertyBinding != null)
                {
                    bindings.AddPropertyBinding(
                        propertyInfo.Name,
                        string.IsNullOrEmpty(propertyBinding.ColumnName) ? propertyInfo.Name : propertyBinding.ColumnName);
                }
            }

            return bindings;
        }

        public void AddPropertyBinding(string propertyName, string columnName)
        {
            if (this.propertyBindings.ContainsKey(propertyName))
            {
                throw new InvalidOperationException("A property can only be bound to a single column.");
            }

            this.propertyBindings[propertyName] = columnName;
        }
    }
}
