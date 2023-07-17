namespace WorkTimer.Web.Common
{
    public class AppSettings
    {
        public string ApiUri { get; set; }
        public double SyncTime { get; set; } = 60;
        public int Timeout { get; set; } = 10;
    }
}