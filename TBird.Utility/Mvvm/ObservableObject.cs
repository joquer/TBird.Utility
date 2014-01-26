// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservableObject.cs" company="Mr Smarti Pantz LLC">
//   Copyright 2012 © Mr Smarti Pantz LLC
// </copyright>
// <summary>
//   Defines the ObservableObject type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#if !SILVERLIGHT
#define NOT_SILVERLIGHT
#endif

#if !NETFX_CORE
#define NOT_METRO
#endif

namespace TBird.Utility.Mvvm
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
#if NOT_SILVERLIGHT
    using System.Diagnostics.Contracts;
#endif
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides <see cref="INotifyPropertyChanged"/> and <see cref="INotifyProperyChanging"/> implementations for classes
    /// that have properties that need change notifications.
    /// </summary>
    [DataContract]
    public class ObservableObject :
#if NOT_SILVERLIGHT && NOT_METRO
 INotifyPropertyChanging,
#endif
 INotifyPropertyChanged
    {

        /// <summary>
        /// INotifyPropertyChanged event to signal that a property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

#if NOT_SILVERLIGHT && NOT_METRO
        /// <summary>
        /// INotifyPropertyChanging event to signal that a property is changing.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;
#endif

        /// <summary>
        /// Gets or sets a value indicating whether ThrowOnInvalidPropertyName is set.  If set,
        /// an ArgumentInvalid exception will be thrown, otherwise, <code>Debug.Fail()</code> will be called.
        /// </summary>
        protected bool ThrowOnInvalidPropertyName { get; set; }

        /// <summary>
        /// Call the event handlers for PropertyChanging event based on the string name of the
        /// property.  In debug, the string is checked using reflection to ensure that it is
        /// a valid property.
        /// </summary>
        /// <param name="propertyName">Name of the property that is changing.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "This does not need to be an event.")]
        public void RaisePropertyChanging(string propertyName)
        {
#if NOT_SILVERLIGHT && NOT_METRO
            Contract.Requires(!string.IsNullOrEmpty(propertyName));
            Contract.Ensures(!string.IsNullOrEmpty(propertyName));
            this.VerifyPropertyName(propertyName);
            PropertyChangingEventHandler handler = PropertyChanging;
            if (handler != null)
            {
                handler(this, new PropertyChangingEventArgs(propertyName));
            }
#endif
        }
            
        /// <summary>
        /// Call the event handlers for PropertyChanging event based on a lambda expression
        /// that references the property.  This method is considerably slower than the string
        /// method, but ensures that the property name is valid.
        /// </summary>
        /// <param name="propertyExpression">
        /// The property expression.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <c>propertyExpression</c> is null.
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        [Conditional("NOT_SILVERLIGHT")]
        public void RaisePropertyChanging(LambdaExpression propertyExpression)
        {
#if NOT_SILVERLIGHT && NOT_METRO
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            PropertyChangingEventHandler handler = this.PropertyChanging;
            if (handler == null)
            {
                return;
            }

            var body = propertyExpression.Body as MemberExpression;
            if (body != null)
            {
                var expression = body.Expression as ConstantExpression;
                if (expression != null)
                {
                    handler(this, new PropertyChangingEventArgs(body.Member.Name));
                }
            }
#endif
        }

        /// <summary>
        /// Call the event handlers for PropertyChanged event based on the string name of the
        /// property.  In debug, the string is checked using reflection to ensure that it is
        /// a valid property.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "This does not need to be an event.")]
        public void RaisePropertyChanged(string propertyName)
        {
            Contract.Requires(!string.IsNullOrEmpty(propertyName));
            this.VerifyPropertyName(propertyName);
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Call the event handlers for PropertyChanged event based on a lambda expression
        /// that references the property.  This method is considerably slower than the string
        /// method, but ensures that the property name is valid.
        /// </summary>
        /// <param name="propertyExpression">
        /// The property expression.
        /// </param>
        /// <typeparam name="T">
        /// The type of the expression.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <c>propertyExpression</c> is null.
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler == null)
            {
                return;
            }

            var body = propertyExpression.Body as MemberExpression;
            if (body != null)
            {
                var expression = body.Expression as ConstantExpression;
                if (expression != null)
                {
                    handler(this, new PropertyChangedEventArgs(body.Member.Name));
                }
            }
        }

        /// <summary>
        /// Verifies that the named property is a member of this class.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property to be verified.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If the property is not a member of this class and <code>THrowOnInvalidPropertyName</code>
        /// is set, then an ArgumentException will be throw.
        /// </exception>
        /// <exception cref="ArgumentNullException">Thrown if propertyName is null or empty.</exception>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private void VerifyPropertyName(string propertyName)
        {
#if NOT_SILVERLIGHT
            Contract.Requires(!string.IsNullOrEmpty(propertyName));
            Contract.Ensures(!string.IsNullOrEmpty(propertyName));
#endif

#if NOT_METRO
            if (this.GetType().GetProperty(propertyName) == null)
#else
            if (this.GetType().GetRuntimeProperty(propertyName) == null)
#endif
            {
                string msg = string.Format(CultureInfo.InvariantCulture, "Class {0} does not have {1} property.", this.GetType().FullName, propertyName);

                if (this.ThrowOnInvalidPropertyName)
                {
                    throw new ArgumentException(msg);
                }

#if SILVERLIGHT || NETFX_CORE
                Debug.WriteLine(msg);
#else
                Debug.Fail(msg);
#endif
            }
        }

        protected void ClearEventHandlers()
        {
            if (this.PropertyChanged != null)
            {
                foreach (PropertyChangedEventHandler del in this.PropertyChanged.GetInvocationList())
                {
                    this.PropertyChanged -= del;
                }
            }
        }
    }
}
