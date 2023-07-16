using Microsoft.AspNetCore.Components;
using WorkTimer.Common.Models;
using WorkTimer.Web.Common.Services;
using Timer = System.Timers.Timer;

namespace WorkTimer.App.Pages
{
    public partial class TimerPage : ComponentBase, IDisposable
    {
        [Inject]
        protected LocalStorageService localStorageService { get; set; }

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
                currentPeriod = new WorkPeriod { StartAt = DateTime.UtcNow };
                refreshTimeTimer.Start();
            }
            else
            {
                currentPeriod.EndAt = DateTime.UtcNow;
                refreshTimeTimer.Stop();
                await localStorageService.WritePeriod(currentPeriod);
                await LoadData();
            }
        }

        private void RefreshTimeTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            refreshTimeTimer.Stop();
            Task.Run(async () =>
            {
                CurrentWorkTime = DateTime.UtcNow.Subtract(currentPeriod.StartAt);
                await InvokeAsync(StateHasChanged);
            });
            refreshTimeTimer.Start();
        }

        void IDisposable.Dispose()
        {
            refreshTimeTimer.Stop();
            refreshTimeTimer.Dispose();
        }
    }
}