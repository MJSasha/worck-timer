using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using QuickActions.Common.Specifications;
using WorkTimer.App.Services;
using WorkTimer.Common.Interfaces;
using WorkTimer.Common.Models;

namespace WorkTimer.App.Pages
{
    [Authorize]
    public partial class CalendarPage : ComponentBase
    {
        [Inject]
        protected ExceptionsHandler exceptionsHandler { get; set; }

        [Inject]
        protected IWorkPeriod workPeriodService { get; set; }

        private DateTime SelectedDate { get; set; }
        private Dictionary<int, double> MonthStatistic { get; set; }
        private DateTime SelectedDay { get; set; }
        private List<WorkPeriod> DayPeriods { get; set; }
        private bool IsLoading { get => isLoading; set { isLoading = value; StateHasChanged(); } }

        private bool isLoading;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            SelectedDate = DateTime.Today;
            await LoadData();
            await ShowDayInfo(DateTime.Today.Day);
        }

        private async Task LoadData()
        {
            IsLoading = true;

            try
            {
                MonthStatistic = await workPeriodService.GetMonthStatistic(SelectedDate);
            }
            catch (Exception ex)
            {
                await exceptionsHandler.Handle(ex);
            }

            IsLoading = false;
        }

        private async Task ShowDayInfo(int dayNumber)
        {
            if (SelectedDay.Day == dayNumber && SelectedDay.Month == SelectedDate.Month && SelectedDay.Year == SelectedDate.Year) return;
            IsLoading = true;

            try
            {
                // TODO - add current user to filter
                var selectedDate = new DateTime(SelectedDate.Year, SelectedDate.Month, dayNumber).Date;
                DayPeriods = await workPeriodService.Read(new Specification<WorkPeriod>(sp => sp.StartAt >= selectedDate.ToUniversalTime() && sp.EndAt < selectedDate.AddDays(1).ToUniversalTime()), 0, int.MaxValue);
                if (DayPeriods == null || !DayPeriods.Any())
                {
                    IsLoading = false;
                    return;
                }
                SelectedDay = selectedDate;
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