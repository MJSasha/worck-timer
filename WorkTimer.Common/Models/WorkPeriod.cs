namespace WorkTimer.Common.Models
{
    public class WorkPeriod
    {
        public int Id { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public bool Synced { get; set; }
        public DateTime SyncedAt { get; set; } = DateTime.UtcNow;
    }
}