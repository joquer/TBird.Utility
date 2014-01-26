// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventAggregator.cs" company="Mr Smarti Pantz LLC">
//   Copyright 2011 Mr Smarti Pantz LLC
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#if !SILVERLIGHT
#define NOT_SILVERLIGHT
#endif

namespace TBird.Utility.Mvvm
{
    using System;
    using System.Collections.Generic;
#if NOT_SILVERLIGHT
    using System.Diagnostics.Contracts;
#endif
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    /// <summary>
    /// This class implements a simple mechanism for sending events to various objects.  To subcribe to an event
    /// a class must implement the ISubscriber&lt;T&gt; interface for each event type that it wants notifications
    /// for.  Then, the class must call EventAgregator.Instance.Subscribe(this) to receive notifications.  The
    /// EventAgregator.Instance.Publish&lt;T&gt;(e) can be called and the event will be sent to all subscribed classes.
    /// </summary>
    public class EventAggregator : IEventAggregator
    {
        /// <summary>
        /// The list of subscribers for each message type.
        /// </summary>
        private readonly Dictionary<Type, List<object>> eventSubscribers = new Dictionary<Type, List<object>>();

        /// <summary>
        /// An object used to lock the <see cref="eventSubscribers"/> object.
        /// </summary>
        private readonly object lockObject = new object();

        /// <summary>
        /// The field behind the singleton property <see cref="Instance"/>.
        /// </summary>
        private static EventAggregator instance;

        /// <summary>
        /// Prevents a default instance of the <see cref="EventAggregator"/> class from being created.
        /// </summary>
        private EventAggregator()
        {
        }

        /// <summary>
        /// Gets the instance of this singleton object.
        /// </summary>
        public static EventAggregator Instance
        {
            get
            {
                return instance ?? (instance = new EventAggregator());
            }
        }

        /// <summary>
        /// Subscribes the specified subscriber.  It will be subscribed to each type that it has an ISubscriber interface for.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Code Contracts validate the arguments.")]
        public void Subscribe(object subscriber)
        {
            lock (this.lockObject)
            {
#if NETFX_CORE
                var subscriberTypes =
                    subscriber.GetType()
                              .GetTypeInfo()
                              .ImplementedInterfaces
                              .Where(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(ISubscriber<>));
#else
                var subscriberTypes =
                    subscriber.GetType().GetInterfaces().Where(
                        i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISubscriber<>));
#endif

                foreach (Type t in subscriberTypes)
                {
                    List<object> subscribers = this.GetSubscribers(t);
                    subscribers.Add(subscriber);
                }
            }
        }

        /// <summary>
        /// Unsubscribes the subscriber from all event notifications
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Code Contracts validate the arguments.")]
        public void Unsubscribe(object subscriber)
        {
#if NETFX_CORE
            var subscriberTypes =
                subscriber.GetType().GetTypeInfo().ImplementedInterfaces.Where(
                    i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(ISubscriber<>));
#else
            var subscriberTypes =
                subscriber.GetType().GetInterfaces().Where(
                    i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISubscriber<>));
#endif
            foreach (Type t in subscriberTypes)
            {
                List<object> subscribers = this.GetSubscribers(t);
                subscribers.Remove(subscriber);
            }
        }

        /// <summary>
        /// Publishes the specified event to all subscribers.  The event will not be sent to the publisher of
        /// the event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="publisher">The publisher.</param>
        /// <param name="eventToPublish">The event to publish.</param>
        public void Publish<TEvent>(object publisher, TEvent eventToPublish)
        {
            Type subscriptionType = typeof(ISubscriber<>).MakeGenericType(typeof(TEvent));
            foreach (object obj in this.GetSubscribers(subscriptionType))
            {
                if (obj == publisher)
                {
                    continue;
                }

                ISubscriber<TEvent> subscriber = (ISubscriber<TEvent>)obj;

                SynchronizationContext syncContext = SynchronizationContext.Current ?? new SynchronizationContext();

                syncContext.Post((s) => subscriber.OnEvent(eventToPublish), null);
            }
        }

        /// <summary>
        /// Gets a list of subscribers to the event type.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns>A list of subscribesr to this event type that implement the ISubscriber&lt;t&gt;.</returns>
        private List<object> GetSubscribers(Type t)
        {
            Contract.Requires(t != null);
            Contract.Ensures(Contract.Result<List<object>>() != null);
            lock (this.lockObject)
            {
                if (!this.eventSubscribers.ContainsKey(t))
                {
                    this.eventSubscribers[t] = new List<object>();
                }

                return this.eventSubscribers[t];
            }
        }
    }
}
