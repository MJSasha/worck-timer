using QuickActions.Common.Data;
using QuickActions.Common.Interfaces;

namespace QuickActions.Web.Identity.Services
{
    public class SessionService<T>
    {
        public Func<T, Task> OnRefreshSession { get; set; }
        public Session<T> SessionData { get; set; }

        private readonly IIdentity<T> identityService;

        public SessionService(IIdentity<T> identityService)
        {
            this.identityService = identityService;
        }

        public async Task RefreshSession()
        {
            SessionData = await identityService.Authenticate();
            if (OnRefreshSession != null) await OnRefreshSession(SessionData.Data);
        }
    }
}