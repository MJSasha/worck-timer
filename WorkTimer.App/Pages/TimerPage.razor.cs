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
        protected LocalStorageService localStorageService { get; set; }

        [Inject]
        protected ExceptionsHandler exceptionsHandler { get; set; }

        [Inject]
        protected TokenAuthStateProvider<User> tokenAuthStateProvider { get; set; }

        [Inject]
        protected NavigationManager navigationManager { get; set; }

        //[Inject]
        //protected IBackgroundService backgroundService { get; set; }

        private TimeSpan CurrentWorkTime { get; set; }
        private List<WorkPeriod> TodayPeriods { get; set; }

        private bool IsLoading { get => isLoading; set { isLoading = value; StateHasChanged(); } }
        private bool TimerIsRunning { get; set; }

        private WorkPeriod currentPeriod;
        private Timer refreshTimeTimer;
        private bool isLoading;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await LoadData();

            refreshTimeTimer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
            refreshTimeTimer.Elapsed += RefreshTimeTimer_Elapsed;
            refreshTimeTimer.AutoReset = false;
        }

        private async Task LoadData()
        {
            IsLoading = true;

            CurrentWorkTime = new();
            TodayPeriods = await localStorageService.ReadPeriods(DateTime.Today, DateTime.Today.AddDays(1).AddMilliseconds(1));

            IsLoading = false;
        }

        private async Task TimerButtonClick(bool value)
        {
            if (TimerIsRunning == value) return;
            TimerIsRunning = value;
            if (TimerIsRunning)
            {
                //backgroundService.StartBackgroundProcess();
                currentPeriod = new WorkPeriod { StartAt = DateTime.UtcNow };
                refreshTimeTimer.Start();
            }
            else
            {
                //backgroundService.StopBackgroundProcess();
                currentPeriod.EndAt = DateTime.UtcNow;
                refreshTimeTimer.Stop();
                await localStorageService.WritePeriod(currentPeriod);
                await LoadData();
            }
        }

        private async Task SyncPeriods()
        {
            try
            {
                IsLoading = true;
                await localStorageService.SyncPeriods();
                await LoadData();
            }
            catch (Exception ex)
            {
                await exceptionsHandler.Handle(ex);
            }
        }

        private async Task LoadPeriods()
        {
            try
            {
                IsLoading = true;
                await localStorageService.LoadPeriods();
                await LoadData();
            }
            catch (Exception ex)
            {
                await exceptionsHandler.Handle(ex);
            }
        }

        private async Task ClearPeriods()
        {
            IsLoading = true;
            await localStorageService.ClearPeriods();
            await LoadData();
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