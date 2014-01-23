// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HolidayDate.cs" company="Mr Smarti Pantz">
//   Copyright 2011 Mr Smarti Pantz LLC
// </copyright>
// <summary>
//   Defines the HolidayDate type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility
{
    using System;

    /// <summary>
    /// This class holds data about a holiday.  It's name, date and whether it is a company holiday.
    /// </summary>
    public class HolidayDate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HolidayDate"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="holidayName">Name of the holiday.</param>
        /// <param name="isCompanyHoliday">if set to <c>true</c> [is company holiday].</param>
        public HolidayDate(DateTime date, string holidayName, bool isCompanyHoliday)
        {
            this.Date           = date;
            this.Name           = holidayName;
            this.CompanyHoliday = isCompanyHoliday;
        }

        /// <summary>
        /// Gets the date for this holiday date instance.
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Gets the name of the holiday.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this holiday is a company holday.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this holiday instance is a company holiday; otherwise, <c>false</c>.
        /// </value>
        public bool CompanyHoliday { get; private set; }
    }
}