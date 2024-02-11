using System.Globalization;

namespace WorkTimer.Web.Common.Utils
{
    public static class Formatters
    {
        public static string Humanize(this TimeSpan timeSpan) => string.Format("{0}:{1}:{2}", (timeSpan.Days * 24 + timeSpan.Hours).ToString("D2"), timeSpan.Minutes.ToString("D2"), timeSpan.Seconds.ToString("D2"));
        public static string Humanize(this DateTime dateTime) => dateTime.ToLocalTime().ToString("d MMMM yyyy HH:mm:ss", CultureInfo.GetCultureInfo("ru-RU"));
        public static string Humanize(this DateTime? dateTime) => dateTime == null ? "" : ((DateTime)dateTime).Humanize();
        public static string HumanizeDate(this DateTime dateTime) => dateTime.ToLocalTime().ToString("d MMMM yyyy", CultureInfo.GetCultureInfo("ru-RU"));
        public static string HumanizeDate(this DateTime? dateTime) => dateTime == null ? "" : ((DateTime)dateTime).HumanizeDate();
        public static string GetMonthName(this DateTime dateTime) => char.ToUpper(dateTime.ToLocalTime().ToString("MMMM", CultureInfo.GetCultureInfo("ru-RU"))[0]) + dateTime.ToLocalTime().ToString("MMMM", CultureInfo.GetCultureInfo("ru-RU"))[1..].ToLower();
        public static string GetMonthName(this DateTime? dateTime) => dateTime == null ? "" : ((DateTime)dateTime).Humanize();
        public static string GetMonthAndYearNames(int year, int month)
        {
            var date = new DateTime(
                year: year,
                month: month,
                day: 1,
                hour: 0,
                minute: 0,
                second: 0,
                kind: DateTimeKind.Local);

            return $"{date.GetMonthName()} {date.Year}";
        }
    }
}