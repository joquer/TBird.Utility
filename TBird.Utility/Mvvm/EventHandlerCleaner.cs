// -----------------------------------------------------------------------
// <copyright file="EventHandlerCleaner.cs" company="Mr Smari Pantz LLC">
//     Copyright © Mr. Smarti Pantz LLC 2011, All rights reserved.
// </copyright>
// <summary>
//   Defines the EventHandlerCleaner type.
// </summary>
// -----------------------------------------------------------------------

#if !NETFX_CORE

namespace TBird.Utility.Mvvm
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// This class provides routines that will remove all event handlers from a class.
    /// It will also print out all event handlers that still have handlers associated with them.
    /// This class helps solve the memory leak problem with WPF and C# that occurs if handlers
    /// are not removed and hold onto references of objects that should be released.  Note, this
    /// does not work for RoutedEvents.
    /// </summary>
    public static class EventHandlerCleaner
    {
        /// <summary>
        /// Constant set of flags for the call into the reflection routines that will return all fields.
        /// </summary>
        private const BindingFlags AllBindings =
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
            BindingFlags.Static;

        /// <summary>
        /// A constant that will be used in the reflection calls to return all non static events.
        /// </summary>
        private const BindingFlags EventFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// This is a cache of the field list that is created for each type.
        /// </summary>
        private static readonly Dictionary<Type, List<FieldInfo>> FieldCache = new Dictionary<Type, List<FieldInfo>>();

        /// <summary>
        /// Gets or sets a value indicating whether handlers will be removed when RemoveHandlers is called.  This allows
        /// a single point to control whether these should be cleaned up or not.  All calling code should include the
        /// call to RemoveHandlers() so that this flag can be used effectively.
        /// </summary>
        public static bool AllowHandlerRemoval { get; set; }

        /// <summary>
        /// Returns the total number of event handlers that have been registered for
        /// all events in this class.
        /// </summary>
        /// <param name="eventObject">The object whose event handlers are to be removed.</param>
        /// <returns>The number of event handlers registered for this class.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "AgumentValidation is handled by Code Contracts")]
        public static int CountEventHandlers(object eventObject)
        {
            Contract.Requires(eventObject != null);
            Contract.Ensures(Contract.Result<int>() >= 0);
            int count = 0;
            Type t = eventObject.GetType();

            foreach (FieldInfo fieldInfo in GetEventFieldInfo(eventObject.GetType()))
            {
                if (fieldInfo == null)
                {
                    continue;
                }

                EventInfo eventInfo = t.GetEvent(fieldInfo.Name, EventFlags);
                if (eventInfo != null)
                {
                    Delegate eventDelegate = fieldInfo.GetValue(eventObject) as Delegate;
                    if (eventDelegate != null)
                    {
                        if (eventDelegate.GetInvocationList().Length > 0)
                        {
                            count += eventDelegate.GetInvocationList().Length;
                        }
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Display a list of all the events for an object and how many event handlers are registered for that event.
        /// </summary>
        /// <param name="eventObject">The object whose events are to be displayed.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Code Contracts validate the arguments.")]
        public static void ShowEventHandlers(object eventObject)
        {
            Contract.Requires(eventObject != null);
            Type t = eventObject.GetType();

            foreach (FieldInfo fieldInfo in GetEventFieldInfo(eventObject.GetType()))
            {
                if (fieldInfo == null)
                {
                    continue;
                }

                EventInfo eventInfo = t.GetEvent(fieldInfo.Name, EventFlags);
                if (eventInfo != null)
                {
                    Delegate eventDelegate = fieldInfo.GetValue(eventObject) as Delegate;
                    if (eventDelegate != null)
                    {
                        if (eventDelegate.GetInvocationList().Length > 0)
                        {
                            Debug.WriteLine(
                                string.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0}.{1}.{2}:  {3} event handlers",
                                    t.Namespace,
                                    t.Name,
                                    fieldInfo.Name,
                                    eventDelegate.GetInvocationList().Length));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Remove all event handlers for an object.
        /// </summary>
        /// <param name="eventObject">The object whose event handlers are to be removed.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Code Contracts validate the arguments.")]
        public static void RemoveEventHandlers(object eventObject)
        {
            Contract.Requires(eventObject != null);

            if (!AllowHandlerRemoval)
            {
                return;
            }

            Type t = eventObject.GetType();
            foreach (FieldInfo fieldInfo in GetEventFieldInfo(eventObject.GetType()))
            {
                if (fieldInfo == null)
                {
                    continue;
                }

                EventInfo eventInfo = t.GetEvent(fieldInfo.Name, EventFlags);
                if (eventInfo != null)
                {
                    Delegate eventDelegate = fieldInfo.GetValue(eventObject) as Delegate;
                    if (eventDelegate != null)
                    {
                        foreach (Delegate del in eventDelegate.GetInvocationList())
                        {
                            eventInfo.RemoveEventHandler(eventObject, del);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finds a field from a type.  It walks the class hierarchy until it finds the field or object is reached.
        /// This is necessary because the Type.GetField method only will find a field for the current class and
        /// will not look at inherited fields.
        /// </summary>
        /// <param name="t">The <c>Type</c> containing the field.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>The <see cref="FieldInfo"/> for the event with this name.</returns>
        private static FieldInfo FindField(Type t, string name)
        {
            Contract.Requires(t != null);
            FieldInfo fi = t.GetField(name, AllBindings);
            if (fi != null)
            {
                return fi;
            }

            if (t.BaseType == typeof(object) || t.BaseType == null)
            {
                return null;
            }

            return FindField(t.BaseType, name);
        }

        /// <summary>
        /// Creates a list of the <see cref="FieldInfo"/> for all the events in the class, including inherited events.
        /// </summary>
        /// <param name="t">The type whose event list is to be created.</param>
        /// <returns>A list of <see cref="FieldInfo"/> for all the events in this class and it's parent classes.</returns>
        private static List<FieldInfo> GetEventFieldInfo(Type t)
        {
            Contract.Requires(t != null);
            if (FieldCache.ContainsKey(t))
            {
                return FieldCache[t];
            }

            List<FieldInfo> eventFields = new List<FieldInfo>();
            foreach (EventInfo eventInfo in t.GetEvents(EventFlags))
            {
                FieldInfo fi = FindField(t, eventInfo.Name);
                if (fi != null)
                {
                    eventFields.Add(fi);
                }
            }

            FieldCache[t] = eventFields;
            return eventFields;
        }
    }
}

#endif
