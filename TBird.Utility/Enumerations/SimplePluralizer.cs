namespace TBird.Utility.Enumerations
{
    /// <summary>
    /// Very simple implentation of a pluralizer.  Only looks at the case of a plural word ending in s, no
    /// special cases considered.
    /// </summary>
    public class SimplePluralizer : IPluralizer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Parameters are validated by code contracts.")]
        public bool IsSingular(string word)
        {
            return !this.IsPlural(word);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Parameters are validated by code contracts.")]
        public string Singularize(string word)
        {
            return this.IsPlural(word) ? word.Substring(0, word.Length - 1) : word;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Parameters are validated by code contracts.")]
        public bool IsPlural(string word)
        {
            return word[word.Length - 1] == 's';
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Parameters are validated by code contracts.")]
        public string Pluralize(string word)
        {
            return word + "s";
        }
    }
}
