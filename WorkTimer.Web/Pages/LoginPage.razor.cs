using Microsoft.AspNetCore.Components;
using QuickActions.Web.Identity;
using QuickActions.Web.Identity.Services;
using WorkTimer.Common.Data;
using WorkTimer.Common.Definitions;
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
                    await sessionService.RefreshSession();
                    navigationManager.NavigateTo(sessionService.SessionData.Data.Role == UserRole.Admin ? Definitons.Pages.Users.GetUrl() : Definitons.Pages.Calendar.GetUrl(), true);
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