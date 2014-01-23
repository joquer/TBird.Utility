// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="Mr Smarti Pantz LLC">
//   Copyright 2012 © Mr Smarti Pantz LLC
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace TBird.Utility.Mvvm
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.Serialization;

    /// <summary>
    /// Base class for ViewModels.  This extends <see cref="ObservableObject"/> and adds IDisposable.  It displays
    /// if an object has been disposed or not and also shows event handlers that have not been removed.
    /// </summary>
    [DataContract]
    public class ViewModelBase : ObservableObject, IDisposable
    {
        /// <summary>
        /// Indicates if the object has been disposed.  Field behind the IsDisposed property.
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// Finalizes an instance of the <see cref="ViewModelBase"/> class. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "The class name is displayed for debugging purposes, then Dispose is called.")]
        ~ViewModelBase()
        {
            if (this.IsDisposed)
            {
                return;
            }

            Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Class {0}.{1} was not disposed", this.GetType().Namespace, this.GetType().Name));
            this.Dispose(false);
        }

        /// <summary>
        /// Gets a value indicating whether the object has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return this.isDisposed; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the
        /// runtime from inside the finalizer and you should not reference
        /// other objects. Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="isDisposing">
        /// Indicates if Dispose is being called from Dispose or from the Finalize method.
        /// </param>
        protected virtual void Dispose(bool isDisposing)
        {
            if (this.IsDisposed == true)
            {
                return;
            }

            if (isDisposing)
            {
                this.ClearEventHandlers();
#if !NETFX_CORE
                EventHandlerCleaner.ShowEventHandlers(this);
                EventHandlerCleaner.RemoveEventHandlers(this);
#endif
            }

            this.isDisposed = true;
        }
    }
}
