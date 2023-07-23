using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using WorkTimer.App.Services;

namespace WorkTimer.App.Pages
{
    [Authorize]
    public partial class CalendarPage : ComponentBase
    {
        [Inject]
        protected ExceptionsHandler exceptionsHandler { get; set; }

        private DateTime SelectedDate { get; set; }
        private bool IsLoading { get => isLoading; set { isLoading = value; StateHasChanged(); } }

        private bool isLoading;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            SelectedDate = DateTime.Today;
            await LoadData();
        }

        private async Task LoadData()
        {
            IsLoading = true;

            try
            {

            }
            catch (Exception ex)
            {
                await exceptionsHandler.Handle(ex);
            }

            IsLoading = false;
        }

        private async Task AddMonth(int monthCount)
        {
            SelectedDate = SelectedDate.AddMonths(monthCount);
            await LoadData();
        }
    }
}