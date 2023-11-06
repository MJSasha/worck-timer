using Microsoft.AspNetCore.Components;
using QuickActions.Web.Identity;
using QuickActions.Web.Identity.Services;
using WorkTimer.Common.Data;
using WorkTimer.Common.Interfaces;
using WorkTimer.Common.Models;

namespace WorkTimer.Web.Pages
{
    public partial class LoginPage : ComponentBase
    {
        [Inject]
        protected SessionCookieService sessionCookieService { get; set; }

        [Inject]
        protected NavigationManager navigationManager { get; set; }

        [Inject]
        protected IUsersIdentity identityService { get; set; }

        [Inject]
        protected SessionService<User> sessionService { get; set; }

        private AuthModel AuthModel { get; set; } = new();
        private bool IsLoading { get => isLoading; set { isLoading = value; StateHasChanged(); } }
        private bool IsLoginFailed { get; set; }

        private bool isLoading;


        protected async Task LoginAsync()
        {
            IsLoading = true;
            try
            {
                var sessionKey = await identityService.Login(AuthModel);

                if (!string.IsNullOrWhiteSpace(sessionKey))
                {
                    await sessionCookieService.WriteSessionKey(sessionKey);
                    navigationManager.NavigateTo(Definitons.Pages.Statistic.GetUrl(), true);
                    await sessionService.RefreshSession();
                }
                else
                {
                    IsLoginFailed = true;
                }
            }
            catch (Exception ex)
            {
                IsLoginFailed = true;
                Console.WriteLine(ex);
                // TODO - add errors handling
            }
            IsLoading = false;
        }
    }
}