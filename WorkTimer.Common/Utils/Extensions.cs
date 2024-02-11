using System.Globalization;
using WorkTimer.Common.Models;

namespace WorkTimer.Common.Utils
{
    public static class Extensions
    {
        public static TimeSpan GetDuration(this IEnumerable<WorkPeriod> periods)
        {
            TimeSpan workDuration = new();
            return periods.Where(p => p.EndAt.HasValue).Select(p => p.EndAt.Value - p.StartAt).GetDuration();
        }

        public static TimeSpan GetDuration(this IEnumerable<TimeSpan> timeSpans)
        {
            TimeSpan workDuration = new();
            foreach (var span in timeSpans)
            {
                workDuration += span;
            }
            return workDuration;
        }

        public static int GetCountWorkDaysInMonth(this DateTime dateTime)
        {
            return dateTime.Month.GetCountWorkDaysInMonth(dateTime.Year);
        }

        public static int GetCountWorkDaysInMonth(this int month, int year)
        {
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            Calendar calendar = cultureInfo.Calendar;
            int daysInMonth = calendar.GetDaysInMonth(year, month);

            int workDays = 0;
            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime date = new DateTime(year, month, day);
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
