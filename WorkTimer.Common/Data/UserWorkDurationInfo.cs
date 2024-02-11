using WorkTimer.Common.Models;

namespace WorkTimer.Common.Data
{
    public class UserWorkDurationInfo
    {
        public User User { get; set; }
        public TimeSpan WorkDuration { get; set; }
    }
}