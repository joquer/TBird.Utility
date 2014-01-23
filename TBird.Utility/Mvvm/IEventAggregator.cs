// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventAggregator.cs" company="Mr Smarti Pantz LLC">
//   Copyright 2013 © Mr Smarti Pantz LLC
// </copyright>
// <summary>
//   Defines the IEventAggregator type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility.Mvvm
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(IEventAggregatorContract))]
    public interface IEventAggregator
    {
        /// <summary>
        /// Subscribes the specified subscriber.  It will be subscribed to each type that it has an ISubscriber interface for.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        void Subscribe(object subscriber);

        /// <summary>
        /// Unsubscribes the subscriber from all event notifications
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        void Unsubscribe(object subscriber);

        /// <summary>
        /// Publishes the specified event to all subscribers.  The event will not be sent to the publisher of
        /// the event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="publisher">The publisher.</param>
        /// <param name="eventToPublish">The event to publish.</param>
        void Publish<TEvent>(object publisher, TEvent eventToPublish);
    }

    [ExcludeFromCodeCoverage]
    [ContractClassFor(typeof(IEventAggregator))]
    public abstract class IEventAggregatorContract : IEventAggregator
    {
        public void Subscribe(object subscriber)
        {
            Contract.Requires(subscriber != null);
            throw new System.NotImplementedException();
        }

        public void Unsubscribe(object subscriber)
        {
            Contract.Requires(subscriber != null);
            throw new System.NotImplementedException();
        }

        public void Publish<TEvent>(object publisher, TEvent eventToPublish)
        {
            Contract.Requires(publisher != null);
            Contract.Requires(eventToPublish != null);
            throw new System.NotImplementedException();
        }
    }
}