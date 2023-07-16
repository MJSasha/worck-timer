using QuickActions.Api;
using WorckTimer.Api.Repository;
using WorkTimer.Common.Models;

namespace WorckTimer.Api.Controllers
{
    public class WorkPeriodController : CrudController<WorkPeriod>
    {
        public WorkPeriodController(WorkPeriodRepository workPeriodRepository) : base(workPeriodRepository)
        {
        }
    }
}
