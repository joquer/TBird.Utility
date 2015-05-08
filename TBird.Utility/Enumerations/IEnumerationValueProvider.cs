// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEnumerationValueProvider.cs" company="Advisory Board Company - Crimson">
//   Copyright © 2014 Advisory Board Company - Crimson
// </copyright>
// <summary>
//   Defines the IEnumerationValueProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(EnumerationValueProviderContract))]
    public interface IEnumerationValueProvider
    {
        /// <summary>       
        /// Gets the values for an Enumeration class.  This routine returns a list of all the values in the Enumeration
        /// from a datasource for the specified table bindings.
        /// </summary>
        /// <param name="bindings">The table bindings for this class.</param>
        /// <returns>A list of <see cref="EnumerationValue"/> containing all the fields in this Enumeration.</returns>
        IEnumerable<EnumerationValue> GetValues(EnumerationTableBindingsAttribute bindings);
    }

    /// <summary>
    /// Code Contracts class for the IEnumerationValueProvider interface
    /// </summary>
    [ExcludeFromCodeCoverage]
    [ContractClassFor(typeof(IEnumerationValueProvider))]
    public abstract class EnumerationValueProviderContract : IEnumerationValueProvider
    {
        public IEnumerable<EnumerationValue> GetValues(EnumerationTableBindingsAttribute bindings)
        {
            Contract.Requires(bindings != null);
            Contract.Ensures(Contract.Result<IEnumerable<EnumerationValue>>() != null);
            throw new NotImplementedException();
        }
    }
}
