using Microsoft.AspNetCore.Mvc;
using QuickActions.Api;
using WorkTimer.Api.Repository;
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
    }
}
