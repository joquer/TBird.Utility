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
