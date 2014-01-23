// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanToVisibilityCollapsedInvertedConverter.cs" company="Mr Smarti Pantz LLC">
//   Copyright 2012 © Mr Smarti Pantz LLC
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility.Converters
{
    using System;
    using System.Globalization;

#if NETFX_CORE
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;
#else
    using System.Windows;
    using System.Windows.Data;
#endif

    /// <summary>
    /// A converter to convert from boolean to Visibility to be able to hide an elment through a bound bool value.
    /// </summary>
    public class BooleanToVisibilityCollapsedInvertedConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
#if NETFX_CORE
        public object Convert(object value, Type targetType, object parameter, string language)
#else
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#endif
        {
            if (!(value is bool))
            {
                return Visibility.Collapsed;
            }

            return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
#if NETFX_CORE
        public object ConvertBack(object value, Type targetType, object parameter, string language)
#else
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#endif
        {
            if (!(value is Visibility))
            {
                return false;
            }

            return (Visibility)value == Visibility.Collapsed ? true : false;
        }

        #endregion
    }
}
