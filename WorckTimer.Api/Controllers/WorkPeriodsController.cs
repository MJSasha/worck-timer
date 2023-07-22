using Microsoft.AspNetCore.Mvc;
using QuickActions.Api;
using QuickActions.Api.Identity.IdentityCheck;
using WorkTimer.Api.Repository;
using WorkTimer.Common.Definitions;
using WorkTimer.Common.Interfaces;
using WorkTimer.Common.Models;

namespace WorkTimer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkPeriodsController : CrudController<WorkPeriod>, IWorkPeriod
    {
        public WorkPeriodsController(WorkPeriodRepository workPeriodRepository) : base(workPeriodRepository)
        {
        }

        [HttpPost("SyncPeriods")]
        [Identity]
        public async Task<List<WorkPeriod>> SyncPeriods([FromBody] List<WorkPeriod> periods)
        {
            periods.ForEach(p => p.Synced = true);
            await Create(periods);
            return periods;
        }
    }
}
