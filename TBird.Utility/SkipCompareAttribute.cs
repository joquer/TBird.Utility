// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SkipCompareAttribute.cs" company="Mr Smarti Pantz">
//   Copyright 2011 Mr Smarti Pantz LLC
// </copyright>
// <summary>
//   Defines the SkipCompareAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility
{
    using System;

    /// <summary>
    /// A simple marker attribute that tells the CompareObject routine to not include
    /// this attribute in the comparison.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SkipCompareAttribute : Attribute
    {
    }
}
