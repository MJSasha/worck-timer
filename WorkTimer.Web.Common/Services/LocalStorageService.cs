using Newtonsoft.Json;
using QuickActions.Common.Specifications;
using System.Timers;
using WorkTimer.Common.Interfaces;
using WorkTimer.Common.Models;
using WorkTimer.Web.Common.Interfaces;
using Timer = System.Timers.Timer;

namespace WorkTimer.Web.Common.Services
{
    public class LocalStorageService : IDisposable
    {
        private readonly string periodStorageKey = "periods-list";
        private readonly IStorageService storageService;
        private readonly IWorkPeriod workPeriodService;

        private readonly Timer timer;

        public LocalStorageService(IStorageService storageService, IWorkPeriod workPeriodService, AppSettings appSettings)
        {
            this.storageService = storageService;
            this.workPeriodService = workPeriodService;

            timer = new Timer(TimeSpan.FromMinutes(appSettings.SyncTime).TotalMilliseconds);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = false;
            timer.Start();
        }

        public async Task WritePeriod(WorkPeriod period)
        {
            var periodsJson = await storageService.Read(periodStorageKey) ?? "";
            var periods = JsonConvert.DeserializeObject<List<WorkPeriod>>(periodsJson) ?? new List<WorkPeriod>();

            periods.Add(period);
            await storageService.Write(periodStorageKey, JsonConvert.SerializeObject(periods));
        }

        public async Task<List<WorkPeriod>> ReadPeriods(DateTime startPeriod, DateTime endPeriod)
        {
            var periodsJson = await storageService.Read(periodStorageKey) ?? "";

            var periods = JsonConvert.DeserializeObject<List<WorkPeriod>>(periodsJson) ?? new List<WorkPeriod>();

            return periods.Where(p => p.StartAt >= startPeriod && p.EndAt <= endPeriod).ToList();
        }

        public async Task SyncPeriods()
        {
            var periodsJson = await storageService.Read(periodStorageKey) ?? "";
            var periods = JsonConvert.DeserializeObject<List<WorkPeriod>>(periodsJson);
            if (periods == null) return;

            var syncedPeriods = await workPeriodService.SyncPeriods(periods.Where(p => !p.Synced).ToList());
            var periodsToSyncCount = periods.RemoveAll(p => !p.Synced);

            if (periodsToSyncCount != syncedPeriods.Count) throw new IOException("Not all periods synced.");

            periods.AddRange(syncedPeriods);
            periods.RemoveAll(p => p.StartAt < DateTime.Today.AddMonths(-2));
            await storageService.Write(periodStorageKey, JsonConvert.SerializeObject(periods));
        }

        public async Task LoadPeriods()
        {
            await SyncPeriods();
            var periods = await workPeriodService.Read(new Specification<WorkPeriod>(wp => wp.StartAt >= DateTime.Today.AddMonths(-2)), 0, int.MaxValue);
            await storageService.Write(periodStorageKey, JsonConvert.SerializeObject(periods));
        }

        public async Task ClearPeriods()
        {
            await storageService.Write(periodStorageKey, JsonConvert.SerializeObject(new List<WorkPeriod>()));
        }

        private async void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            try
            {
                await SyncPeriods();
            }
            catch { }
            timer.Start();
        }

        void IDisposable.Dispose()
        {
            timer.Dispose();
        }
    }
}