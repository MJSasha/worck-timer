using QuickActions.Common.Interfaces;
using Refit;
using WorkTimer.Common.Models;

namespace WorkTimer.Common.Interfaces
{
    public interface IWorkPeriod : ICrud<WorkPeriod>
    {
        [Post("/SyncPeriods")]
        public Task<List<WorkPeriod>> SyncPeriods([Body] List<WorkPeriod> periods);
    }
}
