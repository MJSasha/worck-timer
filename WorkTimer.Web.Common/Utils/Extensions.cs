namespace WorkTimer.Web.Common.Utils
{
    public static class Extensions
    {
        public static int GetDaysUntilNewMonth(this DateTime date)
        {
            DateTime lastDayOfMonth = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            DayOfWeek lastDayOfWeek = lastDayOfMonth.DayOfWeek;
            return (DayOfWeek.Saturday - lastDayOfWeek + 7) % 7;
        }
    }
}