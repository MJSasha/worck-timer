using System.Globalization;

namespace WorkTimer.Web.Common.Utils
{
    public static class Formatters
    {
        public static string Humanize(this TimeSpan timeSpan) => new DateTime(timeSpan.Ticks).ToString("HH:mm:ss");
        public static string Humanize(this DateTime dateTime) => dateTime.ToLocalTime().ToString("d MMMM yyyy HH:mm:ss", CultureInfo.GetCultureInfo("ru-RU"));
    }
}