using Microsoft.AspNetCore.Components.Authorization;
using QuickActions.Common.Interfaces;
using System.Security.Claims;

namespace QuickActions.Web.Identity.Services
{
    public class TokenAuthStateProvider<T> : AuthenticationStateProvider
    {
        private readonly SessionCookieService sessionCookieService;
        private readonly IIdentity<T> identityService;
        private readonly SessionService<T> sessionService;

        public TokenAuthStateProvider(SessionCookieService sessionCookieService, IIdentity<T> identityService, SessionService<T> sessionService)
        {
            this.sessionCookieService = sessionCookieService;
            this.identityService = identityService;
            this.sessionService = sessionService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var sessionKey = await sessionCookieService.ReadSessionKey();
                if (!string.IsNullOrWhiteSpace(sessionKey))
                {
                    await sessionService.RefreshSession();
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Authentication, sessionKey)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "Token");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    return new AuthenticationState(claimsPrincipal);

                }
                return GetStateAnonymous();
            }
            catch
            {
                await sessionCookieService.WriteSessionKey("");
                return GetStateAnonymous();
            }
        }

        public async void SetLogoutState()
        {
            await sessionCookieService.WriteSessionKey("");
            await identityService.Logout();
            NotifyAuthenticationStateChanged(Task.FromResult(GetStateAnonymous()));
        }

        private static AuthenticationState GetStateAnonymous()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            return new AuthenticationState(anonymous);
        }
    }
}