using System.Globalization;

namespace WorkTimer.Web.Common.Utils
{
    public static class Extensions
    {
        public static int GetDaysUntilNewMonth(this DateTime date)
        {
            DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            DayOfWeek firstDayOfWeek = firstDayOfMonth.DayOfWeek;
            int daysUntilNewMonth = ((int)DayOfWeek.Monday - (int)firstDayOfWeek + 7) % 7;
            if (daysUntilNewMonth == 0) return 0;
            else return 7 - daysUntilNewMonth;
        }

        public static int GetCountWorkDaysInMonth(this DateTime dateTime)
        {
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            Calendar calendar = cultureInfo.Calendar;
            int daysInMonth = calendar.GetDaysInMonth(dateTime.Year, dateTime.Month);

            int workDays = 0;
            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime date = new DateTime(dateTime.Year, dateTime.Month, day);
                if (IsWorkDay(date, cultureInfo))
                {
                    workDays++;
                }
            }

            return workDays;
        }

        private static bool IsWorkDay(DateTime date, CultureInfo cultureInfo)
        {
            DayOfWeek dayOfWeek = cultureInfo.Calendar.GetDayOfWeek(date);
            return dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday;
        }
    }
}