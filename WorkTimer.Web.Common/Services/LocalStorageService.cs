using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WorkTimer.Common.Models;
using WorkTimer.Web.Common.Interfaces;

namespace WorkTimer.Web.Common.Services
{
    public class LocalStorageService
    {
        private readonly string periodStorageKey = "periods-list";
        private readonly IStorageService storageService;

        public LocalStorageService(IStorageService storageService)
        {
            this.storageService = storageService;
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
    }
}