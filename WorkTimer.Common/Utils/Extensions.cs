using WorkTimer.Common.Models;

namespace WorkTimer.Common.Utils
{
    public static class Extensions
    {
        public static TimeSpan GetDuration(this IEnumerable<WorkPeriod> periods)
        {
            TimeSpan workDuration = new();
            foreach (var period in periods.Where(p => p.EndAt.HasValue).Select(p => p.EndAt.Value - p.StartAt))
            {
                workDuration += period;
            }
            return workDuration;
        }
    }
}
