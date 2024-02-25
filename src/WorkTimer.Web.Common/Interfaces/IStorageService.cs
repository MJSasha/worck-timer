namespace WorkTimer.Web.Common.Interfaces
{
    public interface IStorageService
    {
        public Task<string> Read(string key);
        public Task Write(string key, string value);
    }
}
