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
    }
}