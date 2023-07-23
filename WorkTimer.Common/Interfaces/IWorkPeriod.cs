using QuickActions.Common.Interfaces;
using Refit;
using WorkTimer.Common.Data;
using WorkTimer.Common.Models;

namespace WorkTimer.Common.Interfaces
{
    public interface IWorkPeriod : ICrud<WorkPeriod>
    {
        [Post("/getMonthStatistic")]
        public Task<Dictionary<int, double>> GetMonthStatistic(DateTime monthDateTime);
    }
}
