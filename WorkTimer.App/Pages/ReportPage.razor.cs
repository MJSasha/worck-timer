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
        protected IWorkPeriod workPeriods { get; set; }

        [Inject]
        protected WorkPeriodsService workPeriodsService { get; set; }

        [Inject]
        protected ExceptionsHandler exceptionsHandler { get; set; }

        private DateTime SelectedMonth { get => selectedMonth; set { selectedMonth = value; LoadData(); } }
        private List<WorkPeriod> WorkPeriods { get; set; }
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
                var startDate = new DateTime(SelectedMonth.Year, SelectedMonth.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                WorkPeriods = await workPeriodsService.LoadPeriods(startDate, endDate);
                MonthStatistic = await workPeriods.GetMonthStatistic(SelectedMonth);
            }
            catch (Exception ex)
            {
                await exceptionsHandler.Handle(ex);
            }

            IsLoading = false;
        }
    }
}