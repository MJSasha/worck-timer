namespace WorkTimer.Common.Data
{
    public class UsersWorksDurationsReportByMonth
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public List<UserWorkDurationInfo> UsersWorksDurationsInfos { get; set; }
    }
}
