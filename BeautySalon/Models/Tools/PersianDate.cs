using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeautySalon.Models.Tools
{
    public static class PersianDate
    {
        /// <summary>
        /// یک استرینگ تاریخ شمسی را به معادل میلادی تبدیل میکند
        /// </summary>
        /// <param name="persianDate">تاریخ شمسی</param>
        /// <returns>تاریخ میلادی</returns>
        public static DateTime ToGeorgianDateTime(this string persianDate)
        {
            string YearInEnglish = ConvertPersianDigistsToEnglishDigists.Convertor(persianDate.Substring(0, 4));
            int year = Convert.ToInt32(YearInEnglish);

            string MontInEnglish = ConvertPersianDigistsToEnglishDigists.Convertor(persianDate.Substring(5, 2));
            int month = Convert.ToInt32(MontInEnglish);

            string DayInEnglish = ConvertPersianDigistsToEnglishDigists.Convertor(persianDate.Substring(8, 2));
            int day = Convert.ToInt32(DayInEnglish);

            DateTime georgianDateTime = new DateTime(year, month, day, new System.Globalization.PersianCalendar());
            return georgianDateTime;
        }

        /// <summary>
        /// یک تاریخ میلادی را به معادل فارسی آن تبدیل میکند
        /// </summary>
        /// <param name="georgianDate">تاریخ میلادی</param>
        /// <returns>تاریخ شمسی</returns>
        public static string ToPersianDateString(this DateTime georgianDate)
        {
            System.Globalization.PersianCalendar persianCalendar = new System.Globalization.PersianCalendar();

            string year = persianCalendar.GetYear(georgianDate).ToString();
            string month = persianCalendar.GetMonth(georgianDate).ToString().PadLeft(2, '0');
            string day = persianCalendar.GetDayOfMonth(georgianDate).ToString().PadLeft(2, '0');
            string persianDateString = string.Format("{0}/{1}/{2}", year, month, day);
            return persianDateString;
        }

        /// <summary>
        /// یک تعداد روز را از یک تاریخ شمسی کم میکند یا به آن آضافه میکند
        /// </summary>
        /// <param name="georgianDate">تاریخ شمسی اول</param>
        /// <param name="days">تعداد روزی که میخواهیم اضافه یا کم کنیم</param>
        /// <returns>تاریخ شمسی به اضافه تعداد روز</returns>
        public static string AddDaysToShamsiDate(this string persianDate, int days)
        {
            DateTime dt = persianDate.ToGeorgianDateTime();
            dt = dt.AddDays(days);
            return dt.ToPersianDateString();
        }
    }
}