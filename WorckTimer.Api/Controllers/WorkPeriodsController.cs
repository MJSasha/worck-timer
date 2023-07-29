using Microsoft.AspNetCore.Mvc;
using QuickActions.Api;
using QuickActions.Api.Identity.IdentityCheck;
using QuickActions.Api.Identity.Services;
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
        private readonly WorkPeriodRepository workPeriodRepository;
        private readonly SessionsService<User> sessionsService;

        public WorkPeriodsController(WorkPeriodRepository workPeriodRepository, SessionsService<User> sessionsService) : base(workPeriodRepository)
        {
            this.workPeriodRepository = workPeriodRepository;
            this.sessionsService = sessionsService;
        }

        [HttpPost("GetMonthStatistic")]
        public async Task<Dictionary<int, double>> GetMonthStatistic(DateTime monthDateTime)
        {
            var currentUser = sessionsService.ReadSession().Data;
            var startDate = new DateTime(monthDateTime.Year, monthDateTime.Month, 1).ToUniversalTime();
            var endDate = startDate.AddMonths(1);

            var periodsFilter = new Specification<WorkPeriod>(wp => wp.StartAt >= startDate.Date && wp.EndAt <= endDate.Date && wp.EndAt != null && wp.UserId == currentUser.Id);
            var periods = await workPeriodRepository.Read(periodsFilter, 0, int.MaxValue);

            var periodGroups = periods.GroupBy(wp => wp.StartAt.Date);

            var result = new Dictionary<int, double>();

            foreach (var periodGroup in periodGroups)
            {
                TimeSpan totalHours = new();
                foreach (var period in periodGroup) totalHours += (DateTime)period.EndAt - period.StartAt;
                int totalSecondsIn24Hours = 24 * 60 * 60;
                int periodSeconds = (int)totalHours.TotalSeconds;
                double percent = (double)periodSeconds / totalSecondsIn24Hours * 100;
                result.Add(periodGroup.Key.Day, percent);
            }

            return result;
        }
    }
}
