namespace WorkTimer.Web.Common.Utils
{
    public static class Extensions
    {
        public static int GetDaysUntilNewMonth(this DateTime date)
        {
            // Получаем последний день текущего месяца.
            DateTime lastDayOfMonth = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

            // Получаем день недели последнего дня текущего месяца.
            DayOfWeek lastDayOfWeek = lastDayOfMonth.DayOfWeek;

            // Вычисляем количество дней в текущей неделе до начала нового месяца.
            int daysUntilNewMonth = (DayOfWeek.Saturday - lastDayOfWeek + 7) % 7;

            return daysUntilNewMonth;
        }
    }
}