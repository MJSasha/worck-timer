using BlazorModalDialogs;
using BlazorModalDialogs.Dialogs.MessageDialog;
using Microsoft.AspNetCore.Components;
using QuickActions.Web.Identity.Services;
using Refit;
using System.Net;
using WorkTimer.Common.Models;

namespace WorkTimer.App.Services
{
    public class ExceptionsHandler
    {
        private readonly TokenAuthStateProvider<User> tokenAuthStateProvider;
        private readonly NavigationManager navigationManager;
        private readonly DialogsService dialogsService;

        public ExceptionsHandler(TokenAuthStateProvider<User> tokenAuthStateProvider, NavigationManager navigationManager, DialogsService dialogsService)
        {
            this.tokenAuthStateProvider = tokenAuthStateProvider;
            this.navigationManager = navigationManager;
            this.dialogsService = dialogsService;
        }

        public async Task Handle(Exception exception)
        {
            if (exception is ApiException apiException)
            {
                if (apiException.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
                {
                    tokenAuthStateProvider.SetLogoutState();
                    navigationManager.NavigateTo("/logout");
                }
            }
            else
            {
                await dialogsService.Show<MessageDialog, MessageDialogParameters, object>(new MessageDialogParameters
                {
                    Title = "Упс...",
                    Message = "Что-то пошло не так",
                    CloseButtonText = "Закрыть"
                });
                Console.WriteLine(exception);
            }
        }
    }
}