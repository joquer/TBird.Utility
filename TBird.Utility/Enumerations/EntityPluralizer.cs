namespace TBird.Utility.Enumerations
{
    using System;
    using System.Data.Entity.Design.PluralizationServices;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// An implementation of IPluralizer that is just a wrapper to the <see cref="Pluralization"/> service 
    /// in System.Data.Entity.Design.PluralizationServices.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EntityPluralizer : IPluralizer
    {
        private static PluralizationService ps = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));

        public EntityPluralizer()
        {
            Contract.Ensures(ps != null);
            if (ps == null)
            {
                throw new InvalidOperationException("The Pluralization Service failed.");
            }
        }

        public bool IsSingular(string word)
        {
            return ps.IsSingular(word);
        }

        public string Singularize(string word)
        {
            return ps.Singularize(word);
        }

        public bool IsPlural(string word)
        {
            return ps.IsPlural(word);
        }

        public string Pluralize(string word)
        {
            return ps.Pluralize(word);
        }
    }
}
