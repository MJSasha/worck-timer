using Microsoft.JSInterop;
using QuickActions.Web.Identity;
using WorkTimer.Web.Common.Interfaces;

namespace WorkTimer.App.Services
{
    public class SessionStorageService : SessionCookieService
    {
        private readonly IStorageService storageService;

        public SessionStorageService(IJSRuntime jSRuntime, string keyName, IStorageService storageService) : base(jSRuntime, keyName)
        {
            this.storageService = storageService;
        }

        public override Task<string> ReadSessionKey()
        {
            return storageService.Read(keyName);
        }

        public override Task WriteSessionKey(string value, int days = 365)
        {
            return storageService.Write(keyName, value);
        }
    }
}