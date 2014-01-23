namespace TBird.Utility.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Defines the functionality for a class that updates enumeration sources.  This interface is used by 
    /// the <see cref="EnumerationUpdater"/> class to update a data source to match the Enumeration definitions
    /// in C# code.
    /// </summary>
    [ContractClass(typeof(EnumerationUpdateProviderContract))]
    public interface IEnumerationUpdateProvider : IEnumerationValueProvider
    {
        /// <summary>
        /// Updates the values for this Enumeration in the data source.
        /// </summary>
        /// <param name="bindings">
        /// The Enumeration bindings for the class.
        /// </param>
        /// <param name="value">
        /// The value of the Enumeration.
        /// </param>
        void UpdateValue(EnumerationTableBindingsAttribute bindings, EnumerationValue value);

        /// <summary>
        /// Inserts an enumeration value into the datasource.
        /// </summary>
        /// <param name="bindings">The bindings.</param>
        /// <param name="value">The value.</param>
        void InsertValue(EnumerationTableBindingsAttribute bindings, EnumerationValue value);

        void Clear(EnumerationTableBindingsAttribute bindings);
    }

    [ExcludeFromCodeCoverage]
    [ContractClassFor(typeof(IEnumerationUpdateProvider))]
    public abstract class EnumerationUpdateProviderContract : IEnumerationUpdateProvider
    {
        public IEnumerable<EnumerationValue> GetValues(EnumerationTableBindingsAttribute bindings)
        {
            throw new NotImplementedException();
        }

        public void UpdateValue(EnumerationTableBindingsAttribute bindings, EnumerationValue value)
        {
            Contract.Requires(bindings != null);
            Contract.Requires(value != null);
            throw new NotImplementedException();
        }

        public void InsertValue(EnumerationTableBindingsAttribute bindings, EnumerationValue value)
        {
            Contract.Requires(bindings != null);
            Contract.Requires(value != null);
            throw new NotImplementedException();
        }

        public void Clear(EnumerationTableBindingsAttribute bindings)
        {
            Contract.Requires(bindings != null);
            throw new NotImplementedException();
        }
    }
}
