using Microsoft.AspNetCore.Components;
using QuickActions.Web.Identity;
using WorkTimer.Common.Data;
using WorkTimer.Common.Interfaces;

namespace WorkTimer.App.Pages
{
    public partial class LoginPage : ComponentBase
    {
        [Inject]
        protected SessionCookieService sessionCookieService { get; set; }

        [Inject]
        protected NavigationManager navigationManager { get; set; }

        [Inject]
        protected IUsersIdentity identityService { get; set; }

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
                    navigationManager.NavigateTo(Definitons.Pages.Timer.GetUrl(), true);
                }
                else
                {
                    IsLoginFailed = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // TODO - add errors handling
            }
            IsLoading = false;
        }
    }
}