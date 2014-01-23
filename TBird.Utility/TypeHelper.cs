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
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Xml.Linq;

#if NETFX_CORE
    using Windows.ApplicationModel;
    using Windows.Storage;
#endif

    ///  <summary>
    ///  This class contains utility routines to parse values into base types, find types and compare types.
    ///  </summary>
    public static partial class TypeHelper
    {
#if !NETFX_CORE
        /// <summary>
        /// Gets the <see cref="Type"/> for the specified name.  This will search all loaded assemblies
        /// instead of just the current Assembly.
        /// </summary>
        /// <param name="fullName">The fully qualified name of the type.</param>
        /// <returns>The type with the specified name, if found; otherwise, Nothing.</returns>
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

        /// <summary>
        /// Compares two strings.  Checks for null strings and tells if a null/non null mismatch occurs.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>A string describing the differences in the two strings.</returns>
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
        /// <param name="type">The type of the object to create.</param>
        /// <returns>A new instance of the specified type.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Code Contracts validate hte arguments.")]
        public static object CreateObject(Type type)
        {
#if NOT_SILVERLIGHT
            Contract.Requires(type != null);
#endif
            ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
            return constructor != null
                               ? constructor.Invoke(new object[] { })
                               : Activator.CreateInstance(type);
        }
#endif


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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "This needs to be reworked, but for now works.")]
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
            if (objType.GetTypeInfo().IsValueType)
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
                            rightValue.Name));
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

#if NETFX_CORE
            foreach (PropertyInfo prop in objType.GetTypeInfo().DeclaredProperties.Where(p => !p.GetCustomAttributes<SkipCompareAttribute>().Any()))
            {
                foreach (string msg in
                    CompareObjects(
                        string.Format(CultureInfo.InvariantCulture, "{0}.{1}", key, prop.Name),
                        prop.GetValue(left, null),
                        prop.GetValue(right, null)))
                {
                    messages.Add(msg);
                }
            }
#else
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
#endif

            return messages;
        }


        /// <summary>
        /// Flattens the Collection of messages into a single string with newlines to separate each message.
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
