// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Holiday.cs" company="Mr Smarti Pantz">
//     Copyright © Mr. Smarti Pantz LLC 2011, All rights reserved.
// </copyright>
// <summary>
//   Defines the Holiday type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /*
January 1 New Year's Day 
January 19 Martin Luther King, Jr. Day (3rd Monday of January, traditionally Jan. 15) 
January 20 Inauguration Day 
February 1 Super Bowl Sunday (currently the first Sunday of February) 
February 2 Groundhog Day 
February 14 Valentine's Day 
February 16 Presidents Day (officially George Washington's Birthday; 3rd Monday of February, traditionally Feb. 22) 
February 25 Ash Wednesday (Christian; moveable based on Easter) 
March 17 St. Patrick's Day 
March 20 Vernal Equinox (based on sun) 
April 1 April Fools' Day 
April 5 Palm Sunday (Christian; Sunday before Easter) 
April 9 First day of Passover (Jewish; moveable based on Jewish calendar) 
April 10 Good Friday (Christian; Friday before Easter) 
April 12 Easter Sunday (Christian; moveable; Sunday after first full moon during spring) 
April 13 Easter Monday (Christian; Monday after Easter) 
April 16 Last Day of Passover (Jewish; moveable, based on Jewish Calendar) 
April 20 Patriot's Day/Marathon Monday/National Weed Day (New England and Wisconsin only)(3rd Monday of April) 
April 22 Earth Day 
April 24 Arbor Day (last Friday of April) 
May 5 Cinco De Mayo (Mexican holiday often observed in US) 
May 10 Mother's Day (2nd Sunday of May), 
May 25 Memorial Day (last Monday of May, traditionally May 30) 
May 31 Pentecost Sunday (Christian; 49 days after Easter) 
June 14 Flag Day 
June 21 Father's Day (3rd Sunday of June), Summer Solstice (based on sun) 
July 4 Independence Day 
August 22 First day of Ramadan (Islamic, moveable based on Lunar calendar) 
September 7 Labor Day (first Monday of September) 
September 11 Patriot Day 
September 13 Grandparents Day (second Sunday of September) 
September 19 Rosh Hashanah (Jewish; moveable, based on Jewish calendar) 
September 20 Last day of Ramadan (Islamic, moveable based on Lunar calendar) 
September 21 Eid-al-Fitr/Day after the end of Ramadan (Islamic, moveable, based on lunar calendar) 
September 22 Autumnal equinox (based on sun) 
September 28 Yom Kippur (Jewish, moveable, 9 days after first day of Rosh Hashanah) 
October 3 First day of Sukkot (Jewish; moveable, 14 days after Rosh Hashanaah) 
October 9 Leif Erikson Day, Last Day of Sukkot (Jewish) 
October 10 Simchat Torah (Jewish; moveable, 22 days after Rosh Hashanah) 
October 12 Columbus Day (2nd Monday of October, traditionally Oct. 12) 
October 30 Mischief Night 
October 31 Halloween 
November 1 All Saints Day 
November 11 Veterans Day 
November 26 Thanksgiving (4th Thursday of November) 
November 27 Black Friday (Friday after Thanksgiving Day) 
December 7 Pearl Harbor Remembrance Day 
December 12 First day of Hanukkah (Jewish; moveable, based on Jewish calendar) 
December 19 Last day of Hanukkah (Jewish; moveable, based on Jewish Calendar) 
December 21 Winter Solstice (based on sun) 
December 24 Christmas Eve (Christian) 
December 25 Christmas Day (Christian) 
December 26 First day of Kwanzaa (Kwanzaa is celebrated until January 1, 2010) 
December 31 New Year's Eve 
     */

    /// <summary>
    /// This class determines a list of holidays for a certain year.  IsHoliday that provides a method to determine
    /// if a specified date is a holiday.
    /// </summary>
    public class Holiday
    {
        /// <summary>
        /// List of holidays for this year.
        /// </summary>
        private readonly List<HolidayDate> holidays = new List<HolidayDate>();

        /// <summary>
        /// List of all holidays with a fixed date. 
        /// The day and date are the only important fields, so using 1900 as the year for all
        /// </summary>
        private readonly HolidayDate[] fixedHolidays =
        {
          new HolidayDate(new DateTime(1900, 1, 1), "New Year's Day", true),
          new HolidayDate(new DateTime(1900, 2, 2), "Ground Hog Day", false),
          new HolidayDate(new DateTime(1900, 2, 14), "Valentine's Day", false),
          new HolidayDate(new DateTime(1900, 3, 17), "St. Patrick's Day", false),
          new HolidayDate(new DateTime(1900, 3, 21), "Vernal Equinox", false),
          new HolidayDate(new DateTime(1900, 4, 22), "Earth Day", false),
          new HolidayDate(new DateTime(1900, 4, 24), "Arbor Day", false),
          new HolidayDate(new DateTime(1900, 5, 5), "Cinco de Mayo", false),
          new HolidayDate(new DateTime(1900, 6, 14), "Flag Day", false),
          new HolidayDate(new DateTime(1900, 6, 21), "Summer Solstice", false),
          new HolidayDate(new DateTime(1900, 7, 4), "Independence Day", true),
          new HolidayDate(new DateTime(1900, 9, 22), "Fall Equinox", false),
          new HolidayDate(new DateTime(1900, 10, 31), "Holloween", false),
          new HolidayDate(new DateTime(1900, 11, 1), "All Saints Day", false),
          new HolidayDate(new DateTime(1900, 11, 11), "Veterans Day", true),
          new HolidayDate(new DateTime(1900, 12, 7), "Pearl Harbor Day", false),
          new HolidayDate(new DateTime(1900, 12, 21), "Winter Solstace", false),
          new HolidayDate(new DateTime(1900, 12, 24), "Christmas Eve", false),
          new HolidayDate(new DateTime(1900, 12, 25), "Christmas Day", true),
          new HolidayDate(new DateTime(1900, 12, 31), "New Year's Eve", true)
        };

        private int year;

        /// <summary>
        /// Initializes a new instance of the <see cref="Holiday"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        public Holiday(int year)
        {
            this.Year = year;
            this.GenerateHolidays();
        }

        /// <summary>
        /// Gets the year.
        /// </summary>
        public int Year
        {
            get
            {
                return this.year;
            }

            private set
            {
                Contract.Requires(this.year >= 1);
                Contract.Requires(this.year < 3000);
                Contract.Ensures(this.year == value);
                this.year = value;
            }
        }

        /// <summary>
        /// Gets the list of holidays.
        /// </summary>
        public IEnumerable<HolidayDate> Days
        {
            get
            {
                return this.holidays;
            }
        }


        /// <summary>
        /// Determines whether the specified day is holiday.
        /// </summary>
        /// <param name="day">The day.</param>
        /// <returns>The HolidayDate if this date is a holiday, or null if it is not.</returns>
        public HolidayDate IsHoliday(DateTime day)
        {
            return this.holidays.FirstOrDefault(h => h.Date.Day == day.Day && h.Date.Month == day.Month);
        }

        /// <summary>
        /// Generates a list of holidays for this instance based on the Year property that is set in the constructor.
        /// </summary>
        private void GenerateHolidays()
        {
            foreach (HolidayDate h in this.fixedHolidays)
            {
                this.holidays.Add(h);
            }

            TimeSpan oneDay = new TimeSpan(1, 0, 0, 0);
            bool done = false;

            // Martin Luther King Day
            DateTime d = new DateTime(this.Year, 1, 1);
            done = false;
            while (done == false)
            {
                if (d.DayOfWeek == DayOfWeek.Monday)
                {
                    done = true;
                }
                else
                {
                    d += oneDay;
                }
            }

            d += new TimeSpan(14, 0, 0, 0);
            HolidayDate hd = new HolidayDate(d, "Martin Luther King Day", true);
            this.holidays.Add(hd);

            // President's Day
            d = new DateTime(this.Year, 2, 1);
            done = false;
            while (done == false)
            {
                if (d.DayOfWeek == DayOfWeek.Monday)
                {
                    done = true;
                }
                else
                {
                    d += oneDay;
                }
            }

            d += new TimeSpan(14, 0, 0, 0);
            hd = new HolidayDate(d, "President's Day", true);
            this.holidays.Add(hd);

            // Mother's Day
            d = new DateTime(this.Year, 5, 1);
            done = false;
            while (done == false)
            {
                if (d.DayOfWeek == DayOfWeek.Sunday)
                {
                    done = true;
                }
                else
                {
                    d += oneDay;
                }
            }

            d += new TimeSpan(7, 0, 0, 0);
            hd = new HolidayDate(d, "Mother's Day", false);
            this.holidays.Add(hd);

            // Memorial Day
            d = new DateTime(this.Year, 5, 31);
            done = false;
            while (done == false)
            {
                if (d.DayOfWeek == DayOfWeek.Monday)
                {
                    done = true;
                }
                else
                {
                    d -= oneDay;
                }
            }

            hd = new HolidayDate(d, "Memorial Day", true);
            this.holidays.Add(hd);

            // Father's Day
            d = new DateTime(this.Year, 6, 1);
            done = false;
            while (done == false)
            {
                if (d.DayOfWeek == DayOfWeek.Sunday)
                {
                    done = true;
                }
                else
                {
                    d += oneDay;
                }
            }

            d += new TimeSpan(14, 0, 0, 0);
            hd = new HolidayDate(d, "Father's Day", false);
            this.holidays.Add(hd);
            d = new DateTime(this.Year, 9, 1);
            done = false;
            while (done == false)
            {
                if (d.DayOfWeek == DayOfWeek.Monday)
                {
                    done = true;
                }
                else
                {
                    d += oneDay;
                }
            }

            hd = new HolidayDate(d, "Labor Day", true);
            this.holidays.Add(hd);
            d = new DateTime(this.Year, 10, 1);
            done = false;
            while (done == false)
            {
                if (d.DayOfWeek == DayOfWeek.Monday)
                {
                    done = true;
                }
                else
                {
                    d += oneDay;
                }
            }

            d += new TimeSpan(14, 0, 0, 0);
            hd = new HolidayDate(d, "Columbus Day", true);
            this.holidays.Add(hd);
            d = new DateTime(this.Year, 11, 1);
            done = false;
            while (done == false)
            {
                if (d.DayOfWeek == DayOfWeek.Thursday)
                {
                    done = true;
                }
                else
                {
                    d += oneDay;
                }
            }

            d += new TimeSpan(21, 0, 0, 0);
            hd = new HolidayDate(d, "Thanksgiving", true);
            this.holidays.Add(hd);
        }
    }
}
