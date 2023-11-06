using QuickActions.Web.Identity;

namespace WorkTimer.Web.Common.Services
{
    public class CookieHandler : DelegatingHandler
    {
        private readonly SessionCookieService sessionCookieService;

        public CookieHandler(SessionCookieService sessionCookieService)
        {
            this.sessionCookieService = sessionCookieService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var sessionKey = await sessionCookieService.ReadSessionKey();
            if (!string.IsNullOrWhiteSpace(sessionKey)) request.Headers.Add("Authorization", $"{sessionKey}");
            return await base.SendAsync(request, cancellationToken);
        }
    }
}