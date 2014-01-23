namespace TBird.Utility.Enumerations
{
    using System;
#if !NEXTFX_CORE
    using System.Diagnostics.CodeAnalysis;
#endif

    /// <summary>
    /// Tags a property of an enumeration as a value that should be synchronized with the database.
    /// The database will be the same as the Property name, unless another value is specified in the
    /// <see cref="ColumnName"/> property.
    /// </summary>
#if !NETFX_CORE
    [ExcludeFromCodeCoverage]
#endif
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class EnumerationPropertyBindingAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the column that this property is bound to.  If this is not set, it defaults to the property name
        /// </summary>
        /// <value>
        /// The name of the column to which this property is bound.
        /// </value>
        public string ColumnName { get; set; }
    }
}
