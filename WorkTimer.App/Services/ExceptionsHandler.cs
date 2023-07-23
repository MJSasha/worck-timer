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

        public ExceptionsHandler(TokenAuthStateProvider<User> tokenAuthStateProvider, NavigationManager navigationManager)
        {
            this.tokenAuthStateProvider = tokenAuthStateProvider;
            this.navigationManager = navigationManager;
        }

        public async Task Handle(Exception exception)
        {
            if (exception is ApiException apiException)
            {
                if (apiException.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
                {
                    tokenAuthStateProvider.SetLogoutState();
                    navigationManager.NavigateTo(Definitons.Pages.Logout.GetUrl());
                }
            }
            throw exception;
        }
    }
}