namespace TBird.Utility.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Helper routines for Reflection.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Finds the method with the specified name.  Walks the class tree to find the method.  The search stops when the <c>object</c> class is reached.
        /// </summary>
        /// <param name="type">The type to be search.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>The <c>MethodInfo</c> for the method, or null if the method is not found.</returns>
        public static MethodInfo FindMethod(Type type, string methodName)
        {
            Contract.Requires(type != null);
            Contract.Requires(!string.IsNullOrEmpty(methodName));
#if NETFX_CORE
            for (TypeInfo currentType = type.GetTypeInfo(); currentType != null && currentType != typeof(object).GetTypeInfo(); currentType = currentType.BaseType.GetTypeInfo())
            {
                foreach (MethodInfo mi in currentType.DeclaredMethods)
                {
                    if (mi.Name == methodName)
                    {
                        return mi;
                    }
                }
            }
#else
            for (Type currentType = type; currentType != null && currentType != typeof(object); currentType = currentType.BaseType)
            {
                foreach (MethodInfo mi in currentType.GetMethods())
                {
                    if (mi.Name == methodName)
                    {
                        return mi;
                    }
                }
            }
#endif

            return null;
        }

        public static bool IsEnumerationType(Type type)
        {
            return GetTypeHierarchy(type)
                .Where(t => t.IsGenericType)
                .Select(t => t.GetGenericTypeDefinition())
                .Any(t => t == typeof(Enumeration<>));
        }

        private static IEnumerable<Type> GetTypeHierarchy(Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }
    }
}
