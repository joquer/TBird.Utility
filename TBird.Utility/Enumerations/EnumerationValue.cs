namespace TBird.Utility.Enumerations
{
    using System.Collections.Generic;
    using System.Diagnostics;
#if !NEXTFX_CORE
    using System.Diagnostics.CodeAnalysis;
#endif
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    [DebuggerDisplay("{Name}({Value}) - {DisplayName}")]
    public class EnumerationValue
    {
        private Dictionary<string, object> propertyValues = new Dictionary<string, object>();

        public int Value { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public Dictionary<string, object> PropertyValues
        {
            get
            {
                return this.propertyValues;
            }

            set
            {
                this.propertyValues = value ?? new Dictionary<string, object>();
            }
        }

        public static Dictionary<string, object> GetPropertyValues<T>(T enumValue)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
#if NETFX_CORE
            foreach (PropertyInfo propertyInfo in typeof(T).GetTypeInfo().DeclaredProperties.Where(x => x.GetCustomAttributes<EnumerationPropertyBindingAttribute>().Any()))
            {
                if (propertyInfo != null)
                {
                    values[propertyInfo.Name] = propertyInfo.GetValue(enumValue);
                }
            }
#else
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties().Where(x => x.GetCustomAttributes(typeof(EnumerationPropertyBindingAttribute), true).Any()))
            {
                if (propertyInfo != null)
                {
                    values[propertyInfo.Name] = propertyInfo.GetValue(enumValue, new object[0]);
                }
            }
#endif

            return values;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}({1}) - {2}", this.Name, this.Value, this.DisplayName);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            return this.Equals(obj as EnumerationValue);
        }

        public bool Equals(EnumerationValue other)
        {
            if (other == null || this.GetType() != other.GetType())
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.Value == this.Value && other.Name == this.Name && other.DisplayName == this.DisplayName;
        }

#if !(NETFX_CORE || WINDOWS_PHONE)
        [ExcludeFromCodeCoverage]
#endif
        public override int GetHashCode()
        {
            unchecked
            {
                int result = this.Value;
                result = (result * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                result = (result * 397) ^ (this.DisplayName != null ? this.DisplayName.GetHashCode() : 0);
                return result;
            }
        }
    }
}
