// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerationDefaultValueAttribute.cs" company="Advisory Board Company - Crimson">
//   Copyright © 2014 Advisory Board Company - Crimson
// </copyright>
// <summary>
//   Tags a field in an enumeration as the default value for the Enumeration class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility.Enumerations
{
    using System;
#if !NEXTFX_CORE
    using System.Diagnostics.CodeAnalysis;
#endif

    /// <summary>
    /// Tags a field in an enumeration as the default value for the Enumeration class.
    /// </summary>
#if !NETFX_CORE
    [ExcludeFromCodeCoverage]
#endif
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EnumerationDefaultValueAttribute : Attribute
    {
    }
}
