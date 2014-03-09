// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelayCommand.cs" company="Mr Smarti Pantz LLC">
//   Copyright 2013 © Mr Smarti Pantz LLC
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility.Mvvm
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;

    public class RelayCommand : ICommand
    {
        #region Fields

        readonly Action<object> execute;

        readonly Predicate<object> canExecute;

        #endregion // Fields

#if WINDOWS_PHONE || NETFX_CORE

        public event EventHandler CanExecuteChanged;

#else

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this.canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (this.canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

#endif

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">
        /// The execute.
        /// </param>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            Contract.Requires(execute != null);
            this.execute    = execute;
            this.canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        #endregion // ICommand Members
    }
}
