// -----------------------------------------------------------------------
// <copyright file="ISubscriber.cs" company="Mr Smarti Pantz LLC">
// Copyright 2011` Mr Smarti Pantz LLC
// </copyright>
// -----------------------------------------------------------------------

namespace TBird.Utility.Mvvm
{
    /// <summary>
    /// Subscribe to all events of a certain type.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface ISubscriber<in TEvent>
    {
        /// <summary>
        /// Called when an event of type <c>TEvent</c> is published.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Can't use event as it is a reserved word.")]
        void OnEvent(TEvent e);
    }
}
