using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace WorkTimer.App.Pages
{
    [Authorize]
    public partial class CalendarPage : ComponentBase
    {
        private DateTime SelectedDate { get; set; }
        private bool IsLoading { get => isLoading; set { isLoading = value; StateHasChanged(); } }

        private bool isLoading;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            SelectedDate = DateTime.Today;
        }
    }
}