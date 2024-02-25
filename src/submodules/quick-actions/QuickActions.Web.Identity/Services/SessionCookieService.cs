using Microsoft.JSInterop;

namespace QuickActions.Web.Identity
{
    public class SessionCookieService
    {
        protected readonly string keyName;
        private readonly IJSRuntime jSRuntime;

        public SessionCookieService(IJSRuntime jSRuntime, string keyName)
        {
            this.jSRuntime = jSRuntime;
            this.keyName = keyName;
        }

        public virtual async Task WriteSessionKey(string value, int days = 365)
        {
            await jSRuntime.InvokeVoidAsync("WriteCookie.WriteCookie", keyName, value, days);
        }

        public virtual async Task<string> ReadSessionKey()
        {
            return await jSRuntime.InvokeAsync<string>("ReadCookie.ReadCookie", keyName);
        }
    }
}