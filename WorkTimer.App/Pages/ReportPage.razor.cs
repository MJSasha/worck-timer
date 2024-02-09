using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using QuickActions.Common.Specifications;
using WorkTimer.App.Services;
using WorkTimer.Common.Interfaces;
using WorkTimer.Common.Models;

namespace WorkTimer.App.Pages
{
    [Authorize]
    public partial class ReportPage : ComponentBase
    {
        [Inject]
        protected IWorkPeriod workPeriodsService { get; set; }

        [Inject]
        protected ExceptionsHandler exceptionsHandler { get; set; }

        private DateTime SelectedMonth { get => selectedMonth;set { selectedMonth = value; LoadData(); } }
        private Dictionary<int, double> MonthStatistic { get; set; }
        private bool IsLoading { get => isLoading; set { isLoading = value; StateHasChanged(); } }

        private DateTime selectedMonth;
        private bool isLoading;

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            await base.OnInitializedAsync();

            SelectedMonth = DateTime.Today;
            await LoadData();
        }

        private async Task LoadData()
        {
            IsLoading = true;

            try
            {
                MonthStatistic = await workPeriodsService.GetMonthStatistic(SelectedMonth);
            }
            catch (Exception ex)
            {
                await exceptionsHandler.Handle(ex);
            }

            IsLoading = false;
        }
    }
}