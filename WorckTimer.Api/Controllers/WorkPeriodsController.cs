using Microsoft.AspNetCore.Mvc;
using QuickActions.Api;
using QuickActions.Api.Identity.IdentityCheck;
using QuickActions.Common.Specifications;
using WorkTimer.Api.Repository;
using WorkTimer.Common.Interfaces;
using WorkTimer.Common.Models;

namespace WorkTimer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [IdentityAll]
    public class WorkPeriodsController : CrudController<WorkPeriod>, IWorkPeriod
    {
        public WorkPeriodsController(WorkPeriodRepository workPeriodRepository) : base(workPeriodRepository)
        {
        }
    }
}
