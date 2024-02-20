using QuickActions.Common.Specifications;
using WorkTimer.Api.Repository;
using WorkTimer.Common.Data;
using WorkTimer.Common.Models;
using WorkTimer.Common.Utils;

namespace WorkTimer.Api.Services
{
    public class WorkPeriodsService
    {
        private readonly WorkPeriodRepository workPeriodRepository;

        public WorkPeriodsService(WorkPeriodRepository workPeriodRepository)
        {
            this.workPeriodRepository = workPeriodRepository;
        }

        public async Task<Dictionary<int, double>> GetMonthStatistic(DateTime monthDateTime, User currentUser)
        {
            var startDate = new DateTime(monthDateTime.Year, monthDateTime.Month, 1).ToUniversalTime();
            var endDate = startDate.AddMonths(1);

            var periodsFilter = new Specification<WorkPeriod>(wp => wp.StartAt >= startDate.Date && wp.EndAt != null && wp.EndAt <= endDate.Date && wp.UserId == currentUser.Id);
            var periods = await workPeriodRepository.Read(periodsFilter, 0, int.MaxValue);

            var periodGroups = periods.GroupBy(wp => wp.StartAt.Date);

            var result = new Dictionary<int, double>();

            foreach (var periodGroup in periodGroups)
            {
                TimeSpan totalHours = new();
                foreach (var period in periodGroup) totalHours += (DateTime)period.EndAt - period.StartAt;
                int totalSecondsInDay = (int)TimeSpan.FromDays(1).TotalSeconds;
                int periodSeconds = (int)totalHours.TotalSeconds;
                double percent = (double)periodSeconds / totalSecondsInDay * 100;
                result.Add(periodGroup.Key.Day, percent);
            }

            return result;
        }

        public async Task<List<UsersWorksDurationsReportByMonth>> GetUsersWorksDurationsReportByMonth(DateTime startAt, DateTime endAt, int? userId)
        {
            var specification = new Specification<WorkPeriod>(s => s.EndAt != null && s.StartAt.Date <= endAt && s.StartAt.Date > startAt);
            if (userId.HasValue) specification &= new Specification<WorkPeriod>(s => s.UserId == userId.Value);
            var periods = await workPeriodRepository.Read(specification.Include(p => p.User), 0, int.MaxValue);

            var periodsGroups = periods.GroupBy(p => new { p.StartAt.Year, p.StartAt.Month, });

            var result = new List<UsersWorksDurationsReportByMonth>();

            foreach (var periodGroup in periodsGroups)
            {
                result.Add(new UsersWorksDurationsReportByMonth
                {
                    Year = periodGroup.Key.Year,
                    Month = periodGroup.Key.Month,
                    UsersWorksDurationsInfos = periodGroup.GroupBy(p => p.User).Select(g => new UserWorkDurationInfo
                    {
                        User = g.Key,
                        WorkDuration = g.GetDuration(),
                        TotalSalary = CalculateTotalSalary(g.GetDuration(), g.Key.Salary, periodGroup.Key.Month, periodGroup.Key.Year)
                    }).ToList(),
                });
            }

            result.ForEach(r => r.TotalSalary = r.UsersWorksDurationsInfos.Sum(i => i.TotalSalary));
            result = result.OrderByDescending(r => r.Year).ThenByDescending(r => r.Month).ToList();

            return result;
        }

        private decimal CalculateTotalSalary(TimeSpan workDuration, decimal salary, int month, int year)
        {
            var workDaysCount = month.GetCountWorkDaysInMonth(year);

            return (salary / (workDaysCount * 8)) * (decimal)workDuration.TotalHours;
        }
    }
}
