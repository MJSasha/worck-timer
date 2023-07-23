using System;
using System.Globalization;

namespace WorkTimer.Web.Common.Utils
{
    public static class Formatters
    {
        public static string Humanize(this TimeSpan timeSpan) => new DateTime(timeSpan.Ticks).ToString("HH:mm:ss");
        public static string Humanize(this DateTime dateTime) => dateTime.ToLocalTime().ToString("d MMMM yyyy HH:mm:ss", CultureInfo.GetCultureInfo("ru-RU"));
        public static string Humanize(this DateTime? dateTime) => dateTime == null ? "" : ((DateTime)dateTime).Humanize();
        public static string HumanizeDate(this DateTime dateTime) => dateTime.ToLocalTime().ToString("d MMMM yyyy", CultureInfo.GetCultureInfo("ru-RU"));
        public static string HumanizeDate(this DateTime? dateTime) => dateTime == null ? "" : ((DateTime)dateTime).HumanizeDate();
        public static string GetMonthName(this DateTime dateTime) => char.ToUpper(dateTime.ToLocalTime().ToString("MMMM", CultureInfo.GetCultureInfo("ru-RU"))[0]) + dateTime.ToLocalTime().ToString("MMMM", CultureInfo.GetCultureInfo("ru-RU"))[1..].ToLower();
        public static string GetMonthName(this DateTime? dateTime) => dateTime == null ? "" : ((DateTime)dateTime).Humanize();
    }
}