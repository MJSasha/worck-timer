using Microsoft.AspNetCore.Mvc;
using QuickActions.Api;
using QuickActions.Api.Identity.IdentityCheck;
using QuickActions.Api.Identity.Services;
using WorkTimer.Api.Repository;
using WorkTimer.Api.Services;
using WorkTimer.Common.Data;
using WorkTimer.Common.Interfaces;
using WorkTimer.Common.Models;

namespace WorkTimer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [IdentityAll]
    public class WorkPeriodsController : CrudController<WorkPeriod>, IWorkPeriod
    {
        private readonly WorkPeriodsService workPeriodsService;
        private readonly SessionsService<User> sessionsService;

        public WorkPeriodsController(WorkPeriodRepository workPeriodRepository, WorkPeriodsService workPeriodsService, SessionsService<User> sessionsService) : base(workPeriodRepository)
        {
            this.workPeriodsService = workPeriodsService;
            this.sessionsService = sessionsService;
        }


        [HttpPost("GetMonthStatistic")]
        public async Task<Dictionary<int, double>> GetMonthStatistic(DateTime monthDateTime)
        {
            var currentUser = sessionsService.ReadSession().Data;
            return await workPeriodsService.GetMonthStatistic(monthDateTime, currentUser);
        }

        [HttpPost("getUsersWorksDurationsReportByMonth")]
        public async Task<List<UsersWorksDurationsReportByMonth>> GetUsersWorksDurationsReportByMonth(DateTime startAt, DateTime endAt, int? userId = null)
        {
            return await workPeriodsService.GetUsersWorksDurationsReportByMonth(startAt, endAt, userId);
        }
    }
}
