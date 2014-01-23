namespace TBird.Utility.Enumerations
{
    using System;
    using System.Data.Entity.Design.PluralizationServices;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    public static class Pluralizer
    {
        [ExcludeFromCodeCoverage]
        public static PluralizationService GetService()
        {
            PluralizationService ps = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));
            if (ps == null)
            {
                throw new InvalidOperationException("The Plurilalization Service failed.");
            }

            return ps;
        }
    }
}
