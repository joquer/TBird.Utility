// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LockObject.cs" company="Mr Smarti Pantz">
//     Copyright © Mr. Smarti Pantz LLC 2011, All rights reserved.//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility
{
#if !NETFX_CORE
    using System.Diagnostics.CodeAnalysis;
#endif

    /// <summary>
    /// The <c>LockObject</c> is a simple class that allows a boolean value to be placed
    /// in a <c>lock</c> block.  In order to use a <c>lock</c> statement, the object must
    /// be a class it can't be a reference type, so this class wraps a boolean and allows
    /// it to be locked.
    /// </summary>
#if !NETFX_CORE
    [ExcludeFromCodeCoverage]
#endif
    public class LockObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LockObject"/> class. 
        /// </summary>
        public LockObject()
        {
            this.IsSet = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the state of the boolean value, defaults to <c>false</c>.
        /// </summary>
        public bool IsSet { get; set; }

        /// <summary>
        /// Sets the value of the <c>LockObject</c> to <c>true</c>.
        /// </summary>
        public void Set()
        {
            this.IsSet = true;
        }

        /// <summary>
        /// Sets the value of the <c>LockObject</c> to <c>false</c>.
        /// </summary>
        public void Reset()
        {
            this.IsSet = false;
        }
    }

}
