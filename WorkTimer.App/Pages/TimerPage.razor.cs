using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using Timer = System.Timers.Timer;

namespace WorkTimer.App.Pages
{
    public partial class TimerPage : ComponentBase, IDisposable
    {
        private TimeSpan CurrentWorkTime { get; set; }
        private bool TimerIsRunning { get; set; }
        
        private Stopwatch stopWatch;
        private Timer refreshTimeTimer;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            refreshTimeTimer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
            refreshTimeTimer.Elapsed += RefreshTimeTimer_Elapsed;
            refreshTimeTimer.Start();
            stopWatch = new Stopwatch();
        }

        private void TimerButtonClick(bool value)
        {
            if (TimerIsRunning == value) return;
            TimerIsRunning = value;
            if (TimerIsRunning)
            {
                refreshTimeTimer.Start();
                stopWatch.Start();
            }
            else
            {
                refreshTimeTimer.Stop();
                stopWatch.Stop();
            }
        }

        private void RefreshTimeTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            refreshTimeTimer.Stop();
            Task.Run(async () =>
            {
                CurrentWorkTime = stopWatch.Elapsed;
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