namespace WorkTimer.Web.Common.Utils
{
    public static class Formatters
    {
        public static string Humanize(this TimeSpan timeSpan) => new DateTime(timeSpan.Ticks).ToString("HH:mm:ss");
    }
}