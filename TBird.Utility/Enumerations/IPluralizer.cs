namespace TBird.Utility.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(PluralizerContract))]
    public interface IPluralizer
    {
        bool IsSingular(string word);

        string Singularize(string word);

        bool IsPlural(string word);

        string Pluralize(string word);
    }

#if !NETFX_CORE
    [ExcludeFromCodeCoverage]
#endif
    [ContractClassFor(typeof(IPluralizer))]
    public abstract class PluralizerContract : IPluralizer
    {
        public bool IsSingular(string word)
        {
            Contract.Requires(!string.IsNullOrEmpty(word));
            throw new NotImplementedException();
        }

        public string Singularize(string word)
        {
            Contract.Requires(!string.IsNullOrEmpty(word));
            throw new NotImplementedException();
        }

        public bool IsPlural(string word)
        {
            Contract.Requires(!string.IsNullOrEmpty(word));
            throw new NotImplementedException();
        }

        public string Pluralize(string word)
        {
            Contract.Requires(!string.IsNullOrEmpty(word));
            throw new NotImplementedException();
        }
    }
}