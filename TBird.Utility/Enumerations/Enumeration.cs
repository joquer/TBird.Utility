namespace TBird.Utility.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// A more full featured Enumeration class based on the Java Enumeration class.  
    /// <para></para>
    /// A default value for the class can be specified using Attributes.  The <see cref="EnumerationDefaultValueAttribute">EnumerationDefaultValue</see> attribute
    /// can be specified on one of the Enumeration Fields and this will be used as the default value.  The <see cref="DefaultValue"/> returns the default value or
    /// null if none is specified.  Also, the routines, <see cref="FromIntWithDefault"/>, <see cref="FromStringWithDefault"/>,
    /// and <see cref="FromDisplayNameWithDefault"/> will return the specified value or the DefaultValue if an invalid value is passed
    /// </summary>
    /// <typeparam name="TEnumeration">The type of the enumeration.</typeparam>
    [DebuggerDisplay("{Name}({Value}) - {DisplayName}")]
#if !NETFX_CORE
    [Serializable]
#endif
    public abstract class Enumeration<TEnumeration> : IComparable<TEnumeration>, IEquatable<TEnumeration>
        where TEnumeration : Enumeration<TEnumeration>
    {
        private static readonly Lazy<TEnumeration[]> LazyEnumerations = new Lazy<TEnumeration[]>(GetEnumerations);

        private static readonly Lazy<TEnumeration> LazyDefaultValue = new Lazy<TEnumeration>(SetDefaultValue);

        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Enumeration&lt;TEnumeration&gt;"/> class.
        /// </summary>
        /// <param name="value">The integer value of the enum.</param>
        protected Enumeration(int value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Enumeration&lt;TEnumeration&gt;"/> class.
        /// </summary>
        /// <param name="value">The integer value of the enum.</param>
        /// <param name="displayName">The string to be displayed in the UI.</param>
        protected Enumeration(int value, string displayName)
        {
            this.Value       = value;
            this.DisplayName = displayName;
        }

        /// <summary>
        /// Gets the list of enumerations for this class.
        /// </summary>
        public static IEnumerable<TEnumeration> Enumerations
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<TEnumeration>>() != null);
                return LazyEnumerations.Value;
            }
        }

        /// <summary>
        /// Gets the default value for the Enumeration, or null if none has been specified.
        /// The default value is specified by decorating one of the fields with the
        /// <see cref="EnumerationDefaultValueAttribute"/>.
        /// </summary>
        public static TEnumeration DefaultValue
        {
            get { return LazyDefaultValue.Value; }
        }

        /// <summary>
        /// Gets or sets the display name for this enumeration
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; protected set; }

        /// <summary>
        /// Gets or sets the integer value of this enum.
        /// </summary>
        /// <value>
        /// The integer value of this enum.
        /// </value>
        public int Value { get; protected set; }

        /// <summary>
        /// Gets the Name of the Enumeration.  This is the same as the field name.
        /// </summary>
        /// <value>
        /// The name of the Enumeration.
        /// </value>
        public string Name
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() == this.Name);

                // Since GetEnumerations() loads the name of the enum into the enum class,
                // name will be null before this is run, so if the lazy loading of enumerations
                // has not happened, then force the loading to get the enums.
                if (!LazyEnumerations.IsValueCreated)
                {
                    LazyEnumerations.Value.Count();
                }

                return this.name;
            }
        }

        /// <summary>
        /// Gets the values for an Enumeration class.  This routine returns a list of all the values in the Enumeration,
        /// as a list <see cref="EnumerationValue"/>.  This is useful for loading and saving enumerations as well as
        /// comparisons.
        /// </summary>
        /// <returns>A list of <see cref="EnumerationValue"/> containing all the fields in this Enumeration.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This is called by reflection and needs to be a Method.")]
        [Pure]
        public static IEnumerable<EnumerationValue> GetValues()
        {
            Contract.Ensures(Contract.Result<IEnumerable<EnumerationValue>>() != null);
            return
                Enumerations.Select(
                    e =>
                    new EnumerationValue()
                    {
                        Value = e.Value,
                        Name = e.Name,
                        DisplayName = e.DisplayName,
                        PropertyValues = EnumerationValue.GetPropertyValues<TEnumeration>(e)
                    });
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Code Contract validates argument.")]
        [Pure]
        public static implicit operator int(Enumeration<TEnumeration> enumValue)
        {
            Contract.Requires(enumValue != null);
            return enumValue.Value;
        }

        [Pure]
        private static bool TryParse(Func<TEnumeration, bool> predicate, out TEnumeration result)
        {
            Contract.Requires(predicate != null);
            result = Enumerations.FirstOrDefault(predicate);
            return result != null;
        }

        [Pure]
        private static TEnumeration Parse(object value, string description, Func<TEnumeration, bool> predicate)
        {
            TEnumeration result;

            if (!TryParse(predicate, out result))
            {
                string message = string.Format(CultureInfo.InvariantCulture, "'{0}' is not a valid {1} in {2}", value, description, typeof(TEnumeration));
                throw new ArgumentException(message, "value");
            }

            return result;
        }

        [Pure]
        public static bool IsValidName(string name)
        {
            TEnumeration result;
            return TryParse(e => e.Name == name, out result);
        }

        [Pure]
        public static TEnumeration FromString(string name)
        {
            return Parse(name, "Name", e => e.Name == name);
        }

        [Pure]
        public static TEnumeration FromStringWithDefault(string name)
        {
            TEnumeration result;
            if (!TryParse(e => e.Name == name, out result))
            {
                result = LazyDefaultValue.Value;
            }

            return result;
        }

        [Pure]
        public static bool IsValidValue(int value)
        {
            TEnumeration result;
            return TryParse(e => e.Value == value, out result);
        }

        [Pure]
        public static TEnumeration FromInt(int value)
        {
            return Parse(value, "Value", e => e.Value == value);
        }

        [Pure]
        public static TEnumeration FromIntWithDefault(int value)
        {
            TEnumeration result;
            if (!TryParse(e => e.Value == value, out result))
            {
                result = LazyDefaultValue.Value;
            }

            return result;
        }

        [Pure]
        public static bool IsValidDisplayName(string displayName)
        {
            TEnumeration result;
            return TryParse(e => e.DisplayName == displayName, out result);
        }

        [Pure]
        public static TEnumeration FromDisplayName(string displayName)
        {
            return Parse(displayName, "DisplayName", e => e.DisplayName == displayName);
        }

        [Pure]
        public static TEnumeration FromDisplayNameWithDefault(string displayName)
        {
            TEnumeration result;
            if (!TryParse(e => e.DisplayName == displayName, out result))
            {
                result = LazyDefaultValue.Value;
            }

            return result;
        }

        [Pure]
        private static TEnumeration[] GetEnumerations()
        {
            Contract.Ensures(Contract.Result<TEnumeration[]>() != null);
#if NETFX_CORE
            TypeInfo enumerationType = typeof(TEnumeration).GetTypeInfo();
            IEnumerable<TEnumeration> enumerationList =
                enumerationType.DeclaredFields.Where(x => x.Attributes == (FieldAttributes.Static | FieldAttributes.Public)  && enumerationType.IsAssignableFrom(x.FieldType.GetTypeInfo()))
                .Select(
                        info =>
                        {
                            TEnumeration e = info.GetValue(null) as TEnumeration;
                            if (e != null)
                            {
                                e.name = info.Name;
                                if (string.IsNullOrEmpty(e.DisplayName))
                                {
                                    e.DisplayName = e.name;
                                }
                            }

                            return e;
                        }).OrderBy(e => e.Value);
#else
            Type enumerationType = typeof(TEnumeration);
            IEnumerable<TEnumeration> enumerationList =
                enumerationType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).Where(
                    info => enumerationType.IsAssignableFrom(info.FieldType)).Select(
                        info =>
                        {
                            TEnumeration e = info.GetValue(null) as TEnumeration;
                            if (e != null)
                            {
                                e.name = info.Name;
                                if (string.IsNullOrEmpty(e.DisplayName))
                                {
                                    e.DisplayName = e.name;
                                }
                            }

                            return e;
                        }).OrderBy(e => e.Value);
#endif

            // Ensure that all the integer values are unique.
            if (enumerationList.Select(e => e.Value).Distinct().Count() != enumerationList.Count())
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "{0} has duplicate values", typeof(TEnumeration).Name));
            }

            return enumerationList.ToArray();
        }

        /// <summary>
        /// Sets the default value private field.  The fields are scanned first to see if one has been tagged as the default value.
        /// If so, the value of that field is returned.  If this is not set, then the class is checked to see if there is a field
        /// called DefaultValue, if so, that value is returned.  If none of these are set, there is no default value and null is
        /// returned.
        /// </summary>
        /// <returns>The default value for this enumeration or null if none of the fields have been tagged as default.</returns>
        private static TEnumeration SetDefaultValue()
        {
            // Added this code to force the initialization of the Enumerations list before DefaultValue is found.
            // The loading of the Enumerations also sets DisplayName to the Name of the Enumeration if no display
            // name was specified.  If this is not set before getting the DefaultValue, then null is returned even
            // if the EnumerationDefaultValueAttribute was specified.
            if (!LazyEnumerations.IsValueCreated)
            {
               TEnumeration[] load = LazyEnumerations.Value;
            }

#if NETFX_CORE
            List<FieldInfo> defaultFields =
                typeof(TEnumeration).GetTypeInfo()
                                    .DeclaredFields.Where(
                                        x => x.GetCustomAttributes(typeof(EnumerationDefaultValueAttribute)).Any())
                                    .ToList();
#else
            List<FieldInfo> defaultFields =
                typeof(TEnumeration).GetFields().Where(
                    f => f.GetCustomAttributes(typeof(EnumerationDefaultValueAttribute), false).Any()).ToList();
#endif
            if (defaultFields.Any())
            {
                if (defaultFields.Count() > 1)
                {
                    StringBuilder message = new StringBuilder();
                    message.AppendFormat("Enumeration {0} has multiple default values:  ", typeof(TEnumeration).Name);
                    foreach (FieldInfo fi in defaultFields)
                    {
                        message.AppendFormat(" {0}", fi == null ? "NullFieldName" : fi.Name);
                    }

                    throw new InvalidOperationException(message.ToString());
                }

                return defaultFields.First().GetValue(null) as TEnumeration;
            }

#if NETFX_CORE
            PropertyInfo prop = typeof(TEnumeration).GetTypeInfo().GetDeclaredProperty("DefaultValue");
#else
            PropertyInfo prop = typeof(TEnumeration).GetProperty("DefaultValue");
#endif
            return (TEnumeration)(prop == null ? null : prop.GetValue(null, new object[0]));
        }

        #region Implementation of IComparable<in TEnumeration>

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        [Pure]
        public int CompareTo(TEnumeration other)
        {
            return this.Value.CompareTo(other.Value);
        }

        #endregion

        #region Implementation of IEquatable<TEnumeration>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="obj"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="obj">An object to compare with this object.</param>
        [Pure]
        public override bool Equals(object obj)
        {
            return obj is int ? this.Value == (int)obj : this.Equals(obj as TEnumeration);
        }

        [Pure]
        public bool Equals(TEnumeration other)
        {
            return other != null && this.Value.Equals(other.Value);
        }

        [Pure]
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        #endregion

        public static bool operator <(Enumeration<TEnumeration> left, Enumeration<TEnumeration> right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return false;
            }

            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return left.Value < right.Value;
        }

        public static bool operator >(Enumeration<TEnumeration> left, Enumeration<TEnumeration> right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return false;
            }

            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return left.Value > right.Value;
        }

        public static bool operator ==(Enumeration<TEnumeration> left, Enumeration<TEnumeration> right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }

            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return left.Value == right.Value;
        }

        public static bool operator !=(Enumeration<TEnumeration> left, Enumeration<TEnumeration> right)
        {
            return !(left == right);
        }
    }
}
