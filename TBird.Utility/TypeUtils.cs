// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeUtils.cs" company="Mr Smarti Pantz LLC">
//   Copyright 2011 Mr Smarti Pantz LLC
// </copyright>
// <summary>
//   This class contains utility routines to parse values into base types, find types and compare types.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Xml.Linq;

    ///  <summary>
    ///  This class contains utility routines to parse values into base types, find types and compare types.
    ///  </summary>
    public static class TypeUtils
    {

        public static decimal ParseDecimal(string decimalText)
        {
            decimal result;
            return decimal.TryParse(decimalText, out result) ? result : 0;
        }

        ///  <summary>
        ///  Find a tag with a specific name and parses it's value as a boolean.
        ///  </summary>
        ///  <param name="el">The attribute containing the boolean tag</param>
        ///  <param name="name">The name of the boolean attribute</param>
        ///  <param name="defaultValue">The default value to be used if the string cannot be parsed as a boolean.</param>
        ///  <returns>returns true/false if the string cannot be parsed as a boolean, the defaultValue is returned.</returns>
        public static bool ParseBoolean(XElement el, string name, bool defaultValue = false)
        {
            if (el == null)
            {
                return defaultValue;
            }

            XAttribute att = el.Attribute(name);
            if (att == null)
            {
                return defaultValue;
            }

            bool value;
            if (bool.TryParse(att.Value, out value))
            {
                return value;
            }

            return defaultValue;
        }

        public static int ParseInt(string intText)
        {
            int number = 0;
            return int.TryParse(intText, out number) ? number : 0;
        }

        /// <summary>
        /// Find an attribute on an XML Element and parse it's value as an int.
        /// </summary>
        /// <param name="el">The attribute containing the int tag.</param>
        /// <param name="name">The name of the attribute containing the integer value.</param>
        /// <param name="defaultValue">The default value to be used if the string cannot be parsed as a int.
        /// If no default value is supplied, zero is used.</param>
        /// <returns>returns the integer value of the string, or defaultValue if the string is not a valid int.</returns>
        public static int ParseInt(XElement el, string name, int defaultValue = 0)
        {
            if (el == null)
            {
                return defaultValue;
            }

            XAttribute att = el.Attribute(name);
            if (att == null)
            {
                return defaultValue;
            }

            int value;
            if (!int.TryParse(att.Value, out value))
            {
                value = defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Find an element an XML Element and parses it's value as an int.
        /// </summary>
        /// <param name="el">The element containing the integer tag.</param>
        /// <param name="ns">THe namespace for this element.</param>
        /// <param name="name">The name of the element with the integer value.</param>
        /// <param name="defaultValue">The default value to be used if the string cannot be parsed as a integer.
        /// If no default value is supplied, zero is used.</param>
        /// <returns>returns the integer value of the string, or defaultValue if the string is not a valid int.</returns>
        public static int ParseInt(XElement el, XNamespace ns, string name, int defaultValue = 0)
        {
            if (el == null)
            {
                return defaultValue;
            }

            XElement valueElement = el.Element(ns + name);
            if (valueElement == null)
            {
                return defaultValue;
            }

            int value;
            if (!int.TryParse(valueElement.Value, out value))
            {
                value = defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Find an attribute on an XML Element and parses it's value as an int.
        /// </summary>
        /// <param name="el">The attribute containing the int tag.</param>
        /// <param name="name">The name of the int attribute</param>
        /// <param name="defaultValue">The default value to be used if the string cannot be parsed as a int.
        /// If no default value is supplied, zero is used.</param>
        /// <returns>returns the integer value of the string, or defaultValue if the string is not a valid int.</returns>
        public static long ParseLong(XElement el, string name, long defaultValue = 0)
        {
            if (el == null)
            {
                return defaultValue;
            }

            XAttribute att = el.Attribute(name);
            if (att == null)
            {
                return defaultValue;
            }

            long value;
            if (!long.TryParse(att.Value, out value))
            {
                value = defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Find a tag with a specific name and parses it's value as an double.
        /// </summary>
        /// <param name="el">The attribute containing the double tag</param>
        /// <param name="name">The name of the double attribute</param>
        /// <param name="defaultValue">The default value to be used if the string cannot be parsed as a double.</param>
        /// <returns>returns the double value of the string, or defaultValue if the string is not a valid double.</returns>
        public static double ParseDouble(XElement el, string name, double defaultValue)
        {
            if (el == null)
            {
                return defaultValue;
            }

            XAttribute att = el.Attribute(name);
            if (att == null)
            {
                return defaultValue;
            }

            double value;
            if (!double.TryParse(att.Value, out value))
            {
                value = defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Find a tag with a specific name and parses it's value as an DateTime.
        /// </summary>
        /// <param name="el">The attribute containing the DateTime tag</param>
        /// <param name="name">The name of the DateTime attribute</param>
        /// <param name="defaultValue">The default value to be used if the string cannot be parsed as a DateTime.</param>
        /// <returns>returns the double value of the string, or defaultValue if the string is not a valid DateTime.</returns>
        public static DateTime ParseDateTime(XElement el, string name, DateTime defaultValue)
        {
            if (el == null)
            {
                return defaultValue;
            }

            XAttribute att = el.Attribute(name);
            if (att == null)
            {
                return defaultValue;
            }

            DateTime value;
            if (!DateTime.TryParse(att.Value, out value))
            {
                value = defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Find a tag with a specific name and parses it's value as an DateTime.
        /// </summary>
        /// <param name="el">The attribute containing the DateTime tag</param>
        ///<param name="ns">The namespace of the tag.</param>
        ///<param name="name">The name of the DateTime attribute</param>
        /// <param name="defaultValue">The default value to be used if the string cannot be parsed as a DateTime.</param>
        /// <returns>returns the double value of the string, or defaultValue if the string is not a valid DateTime.</returns>
        public static DateTime ParseDateTime(XElement el, XNamespace ns, string name, DateTime defaultValue)
        {
            if (el == null)
            {
                return defaultValue;
            }

            XElement dateTimeElement = el.Element(ns + name);
            if (dateTimeElement == null)
            {
                return defaultValue;
            }

            DateTime value;
            if (!DateTime.TryParse(dateTimeElement.Value, out value))
            {
                value = defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Find a tag with a specific name and parses it's value as a Guid.
        /// </summary>
        /// <param name="el">The element containing the Guid tag</param>
        /// <param name="name">The name of the Guid attribute</param>
        /// <returns>returns the Guid value of the string, or Guid.Empty if the string is not a valid Guid.</returns>
        public static Guid ParseGuid(XElement el, string name)
        {
            if (el == null)
            {
                return Guid.Empty;
            }

            XAttribute att = el.Attribute(name);
            if (att == null)
            {
                return Guid.Empty;
            }

            try
            {
                return new Guid(att.Value);
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Find a tag with a specific name and parses it's value as an enum.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="el">The element containing the double tag</param>
        /// <param name="name">The name of the double element</param>
        /// <param name="defaultValue">The default value to be used if the string cannot be parsed as a double.</param>
        /// <returns>returns the double value of the string, or defaultValue if the string is not a valid double.</returns>
        public static T ParseEnum<T>(XElement el, string name, T defaultValue)
        {
            if (el == null)
            {
                return defaultValue;
            }

            XAttribute att = el.Attribute(name);
            if (att == null)
            {
                return defaultValue;
            }

            try
            {
                return (T)Enum.Parse(typeof(T), att.Value, true);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

#if !NETFX_CORE
        /// <summary>
        /// Gets the <see cref="Type"/> for the specified name.  This will seach all loaded assemblies
        /// instead of just the current Assembly.
        /// </summary>
        /// <param name="fullName">The fully qualified name of the type.</param>
        /// <returns>The type with the specified name, if found; otherwise, Nothing.</returns>
        public static Type GetType(string fullName)
        {
            Type t = Type.GetType(fullName);
            if (t != null)
            {
                return t;
            }

            foreach (Assembly assem in AppDomain.CurrentDomain.GetAssemblies())
            {
                t = assem.GetType(fullName);
                if (t != null)
                {
                    return t;
                }
            }

            return null;
        }
#endif

#if !NETFX_CORE
        /// <summary>
        /// Gets a named tag from the XML Document and uses the value of this tag to look up a value from
        /// the type parameter.  The value of this property is then returned.  This is used to allow easy
        /// lookup of values from the system defined classes such as FontStyle or FontWeights.
        /// </summary>
        /// <param name="el">The element whose value will be used as the property name.</param>
        /// <param name="propertyType">The type of the property.</param>
        /// <param name="name">The name of the element.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Returns the value from the property whose name is taken from the value of the named
        /// element, or the defaultValue if either the property or the element are not found.</returns>
        /// <example>
        /// The following code would return FontStyles.Italic.  From the XML 
        /// <code>
        /// <FontStyle>Italic</FontStyle>
        /// XElement el = new XElement("FontStyle", "Italic");
        /// FontStyle style = (FontStyle)TypeUtils.GetPropertyValue(el, typeof(FontStyles), "FontStyle", FontStyles.Normal);
        /// </code></example>
        public static object GetPropertyValue(XElement el, Type propertyType, string name, object defaultValue)
        {
            if (el == null || propertyType == null)
            {
                return defaultValue;
            }

            XAttribute att = el.Attribute(name);
            if (att == null)
            {
                return defaultValue;
            }

            PropertyInfo prop = propertyType.GetProperty(att.Value);
            if (prop == null)
            {
                return defaultValue;
            }

            return prop.GetValue(null, null);
        }
#endif

        /// <summary>
        /// Compares two strings.  Checks for null strings and tells if a null/non null mismatch occurs.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>A string describing the differences in the two strings.</returns>
        public static string CompareString(string fieldName, string left, string right)
        {
            if (left == null && right == null)
            {
                return null;
            }

            if (left == null)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0} mismatch - null != [{1}]", fieldName, right);
            }

            if (right == null)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0} mismatch - [{1}] != null", fieldName, left);
            }

            if (left.Equals(right))
            {
                return null;
            }

            return string.Format(CultureInfo.InvariantCulture, "{0} mismatch - [{1}] != [{2}]", fieldName, left, right);
        }

#if !NETFX_CORE
        /// <summary>
        /// Creates an instance of an object of a specified type.
        /// </summary>
        /// <param name="type">The ttype of the object to create.</param>
        /// <returns>A new instance of the specified type.</returns>
        public static object CreateObject(Type type)
        {
            Contract.Requires(type != null);
            ConstructorInfo constructor = type.GetTypeInfo().GetConstructor(Type.EmptyTypes);
            return constructor != null
                               ? constructor.Invoke(new object[] { })
                               : Activator.CreateInstance(type);
        }
#endif

#if !NETFX_CORE
        /// <summary>
        /// Compares two objects by comparing the properties in each object.  Only properties, both public and private,
        /// are compared, no fields are compared.  It checks for null and non null values and checks that the two
        /// objects are of the same type.  Two null objects are considered equal.
        /// </summary>
        /// <param name="key">A string value used in the description messages when an inequality is found</param>
        /// <param name="left">The left instance of the object.</param>
        /// <param name="right">The right instance of the object.</param>
        /// <returns>
        /// A list of strings that describe the differences in the two objects, or an empty Collection
        /// if the two objects are the same.
        /// </returns>
        public static Collection<string> CompareObjects(string key, object left, object right)
        {
            // First set of checks.  Make sure that either both are null or both are not null.
            if (left == null && right == null)
            {
                return new Collection<string>();
            }

            if (left == null)
            {
                return new Collection<string>()
                    {
                        string.Format(CultureInfo.InvariantCulture, "{0} mismatch - null and {1}", key, right) 
                    };
            }

            if (right == null)
            {
                return new Collection<string>()
                    {
                        string.Format(CultureInfo.InvariantCulture, "{0} mismatch - {1} and null", key, left) 
                    };
            }

            // If both are not null, make sure that the two objects are of the same type.
            if (left.GetType() != right.GetType())
            {
                Type leftType = left.GetType();
                Type rightType = right.GetType();
                if ((leftType.Name.Contains("_Accessor") && leftType.Name == rightType.Name + "_Accessor")
                    || (rightType.Name.Contains("_Accessor") && leftType.Name + "_Accessor" == rightType.Name))
                {
                    return new Collection<string>();
                }

                return new Collection<string>()
                    {
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "{0} mismatch - objects not of same type {1} and {2}",
                            key,
                            left.GetType().FullName,
                            right.GetType().FullName)
                    };
            }

            Collection<string> messages = new Collection<string>();
            Type objType = left.GetType();
            if (objType.IsValueType)
            {
                if (!left.Equals(right))
                {
                    messages.Add(
                        string.Format(CultureInfo.InvariantCulture, "{0} mismatch - {1} != {2}", key, left, right));
                }

                return messages;
            }

            string leftString = left as string;
            if (leftString != null)
            {
                string mesg = CompareString(key, leftString, right as string);
                if (mesg != null)
                {
                    messages.Add(mesg);
                }

                return messages;
            }

            Type leftValue = left as Type;
            if (leftValue != null)
            {
                Type rightValue = right as Type;
                if (rightValue == null)
                {
                    messages.Add(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "{0} mismatch - left is of type {1}, right is null or is not of type Type.",
                            key,
                            leftValue.Name));
                    return messages;
                }

                if ((leftValue.Name.Contains("_Accessor") && leftValue.Name == rightValue.Name + "_Accessor")
                    || (rightValue.Name.Contains("_Accessor") && leftValue.Name + "_Accessor" == rightValue.Name))
                {
                    return messages;
                }

                if (leftValue != rightValue)
                {
                    messages.Add(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "{0} mismatch - {1} != {2}",
                            key,
                            leftValue.Name,
                            rightValue != null ? rightValue.Name : "null"));
                }

                return messages;
            }

            IList leftList = left as IList;
            if (leftList != null)
            {
                IList rightList = right as IList;
                if (rightList == null)
                {
                    messages.Add(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "{0} mismatch - the right value is not an IList or is null",
                            key));
                }
                else if (leftList.Count != rightList.Count)
                {
                    messages.Add(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "{0} mismatch - Lists are not the same size {1} != {2}",
                            key,
                            leftList.Count,
                            rightList.Count));
                }
                else
                {
                    for (int i = 0; i < leftList.Count; i++)
                    {
                        foreach (string msg in
                            CompareObjects(
                                string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", key, i),
                                leftList[i],
                                rightList[i]))
                        {
                            messages.Add(msg);
                        }
                    }
                }

                return messages;
            }

            foreach (PropertyInfo prop in
                objType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                SkipCompareAttribute[] atts =
                    prop.GetCustomAttributes(typeof(SkipCompareAttribute), false) as SkipCompareAttribute[];
                if (atts != null && atts.Length > 0)
                {
                    continue;
                }

                foreach (string msg in
                    CompareObjects(
                        string.Format(CultureInfo.InvariantCulture, "{0}.{1}", key, prop.Name),
                        prop.GetValue(left, null),
                        prop.GetValue(right, null)))
                {
                    messages.Add(msg);
                }
            }

            return messages;
        }
#endif


        /// <summary>
        /// Flattens the Collection of messages into a single string with newlines to seperate each message.
        /// </summary>
        /// <param name="key">The key for this field, used as header in the output string.</param>
        /// <param name="messages">A collection of single line messages.</param>
        /// <returns>The messages as a single string</returns>
        public static string FlattenMessages(string key, IEnumerable<string> messages)
        {
            if (messages == null || !messages.Any())
            {
                return string.Empty;
            }

            StringBuilder bldr = new StringBuilder(key);
            bldr.AppendLine("...");
            foreach (string msg in messages)
            {
                bldr.Append("  ");
                bldr.AppendLine(msg);
            }

            return bldr.ToString();
        }
    }
}
