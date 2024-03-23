namespace QuickActions.Common.Data
{
    public class Session<T>
    {
        public T Data { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}