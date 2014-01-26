namespace TBird.Utility.Enumerations
{
    using System;
#if !(NETFX_CORE || WINDOWS_PHONE)
    using System.Diagnostics.CodeAnalysis;
#endif

    /// <summary>
    /// Tags a field in an enumeration as the default value for the Enumeration class.
    /// </summary>
#if !(NETFX_CORE || WINDOWS_PHONE)
    [ExcludeFromCodeCoverage]
#endif
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EnumerationDefaultValueAttribute : Attribute
    {
    }
}
