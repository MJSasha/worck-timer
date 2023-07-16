using WorkTimer.Web.Common.Interfaces;

namespace WorkTimer.App.Services
{
    public class StorageService : IStorageService
    {
        public Task<string> Read(string key)
        {
            return SecureStorage.Default.GetAsync(key);
        }

        public Task Write(string key, string value)
        {
            return SecureStorage.Default.SetAsync(key, value);
        }
    }
}