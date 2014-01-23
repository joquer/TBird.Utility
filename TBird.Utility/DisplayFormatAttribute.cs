// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayFormatAttribute.cs" company="Mr Smarti Pantz LLC">
//   Copyright 2013 © Mr Smarti Pantz LLC
// </copyright>
// <summary>
//   Defines the DisplayFormatAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility
{
    using System;
#if !NETFX_CORE
    using System.Diagnostics.CodeAnalysis;
#endif
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides an attribute to a class used as a template to allow custom formatting of the value.
    /// </summary>
#if !NETFX_CORE
    [ExcludeFromCodeCoverage]
#endif
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class DisplayFormatAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayFormatAttribute"/> class.
        /// </summary>
        /// <param name="format">
        /// The format string to be used to format the value.
        /// </param>
        public DisplayFormatAttribute(string format)
        {
            Contract.Requires(!string.IsNullOrEmpty(format));
            Contract.Ensures(!string.IsNullOrEmpty(format));
            Contract.Ensures(this.Format == format);
            this.Format = format;
        }

        /// <summary>
        /// Gets the value The format string to be used with string.Format to display the value.
        /// </summary>
        public string Format { get; private set; }
    }
}
