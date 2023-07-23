using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using QuickActions.Web.Identity.Services;
using WorkTimer.App.Services;
using WorkTimer.Common.Models;
using WorkTimer.Web.Common.Services;
using Timer = System.Timers.Timer;

namespace WorkTimer.App.Pages
{
    [Authorize]
    public partial class TimerPage : ComponentBase, IDisposable
    {
        [Inject]
        protected WorkPeriodsService workPeriodsService { get; set; }

        [Inject]
        protected ExceptionsHandler exceptionsHandler { get; set; }

        [Inject]
        protected TokenAuthStateProvider<User> tokenAuthStateProvider { get; set; }

        [Inject]
        protected NavigationManager navigationManager { get; set; }

        private TimeSpan CurrentWorkTime { get; set; }
        private List<WorkPeriod> TodayPeriods { get; set; } = new();

        private bool IsLoading { get => isLoading; set { isLoading = value; StateHasChanged(); } }
        private bool TimerIsRunning { get; set; }

        private WorkPeriod currentPeriod;
        private Timer refreshTimeTimer;
        private bool isLoading;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            currentPeriod = await workPeriodsService.LoadCurrentPeriod();
            await LoadData();

            refreshTimeTimer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
            refreshTimeTimer.Elapsed += RefreshTimeTimer_Elapsed;
            refreshTimeTimer.AutoReset = false;
            if (currentPeriod != null)
            {
                refreshTimeTimer.Start();
                TimerIsRunning = true;
                StateHasChanged();
            }
        }

        private async Task LoadData()
        {
            IsLoading = true;

            TodayPeriods = await workPeriodsService.LoadPeriods(DateTime.Today);

            IsLoading = false;
        }

        private async Task TimerButtonClick(bool value)
        {
            if (TimerIsRunning == value) return;
            TimerIsRunning = value;
            if (TimerIsRunning)
            {
                currentPeriod = await workPeriodsService.StartPeriod();
                refreshTimeTimer.Start();
            }
            else
            {
                refreshTimeTimer.Stop();
                CurrentWorkTime = TimeSpan.Zero;
                await workPeriodsService.CompletePeriod();
                await LoadData();
            }
        }

        private void RefreshTimeTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Task.Run(async () =>
            {
                CurrentWorkTime = DateTime.UtcNow.Subtract(currentPeriod.StartAt);
                await InvokeAsync(StateHasChanged);
            });
            refreshTimeTimer.Start();
        }

        void IDisposable.Dispose()
        {
            refreshTimeTimer?.Dispose();
        }
    }
}