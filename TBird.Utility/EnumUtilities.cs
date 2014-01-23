// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumUtilities.cs" company="">
//     Copyright © Mr. Smarti Pantz LLC 2011, All rights reserved.//   
// </copyright>
// <summary>
//   Defines the EnumUtilities type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Utility routines that allow parsing of Enums like other .Net value types.
    /// </summary>
    public static class EnumUtilities
    {
        /// <summary>
        /// Parse a test string and return it's enum value.
        /// </summary>
        /// <param name="enumType">An enumeration type containing the value.</param>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="result">An object of type <code>enumType</code> whose value is represented by value.</param>
        /// <returns>True if the value was succefully parsed into a value of the enum Type, otherwise false.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "This uses the same signature as the other TryParse methods.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "A generic implementation is also available.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Any failure results in returning a default value for the enum.")]
        public static bool EnumTryParse(Type enumType, string value, out object result)
        {
            Contract.Requires(enumType != null);
            Contract.Requires(!string.IsNullOrEmpty(value));
            Contract.Ensures(!string.IsNullOrEmpty(value));
            try
            {
                result = Enum.Parse(enumType, value);
                return true;
            }
            catch (Exception)
            {
                result = Activator.CreateInstance(enumType);
            }

            return false;
        }

        /// <summary>
        /// Parse a test string and return it's enum value.
        /// </summary>
        /// <typeparam name="T">An enumeration type containing the value.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="result">An object of type <code>enumType</code> whose value is represented by value.</param>
        /// <returns>An object of type T whose value is represented by value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Any failure results in returning a default value for the enum.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "This uses the same signature as the other TryParse methods.")]
        public static bool EnumTryParse<T>(string value, out T result)
        {
            Contract.Requires(!string.IsNullOrEmpty(value));
            try
            {
                result = (T)Enum.Parse(typeof(T), value);
                return true;
            }
            catch (Exception)
            {
                result = (T)Activator.CreateInstance(typeof(T));
            }

            return false;
        }
    }
}
