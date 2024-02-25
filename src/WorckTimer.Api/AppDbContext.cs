using Microsoft.EntityFrameworkCore;
using WorkTimer.Common.Definitions;
using WorkTimer.Common.Models;

namespace WorkTimer.Api
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<WorkPeriod> WorkPeriods { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var users = new List<User>
            {
                new() { Id = 1, Email = "admin", Password = "admin", Role = UserRole.Admin, Name = "admin", Salary = 150000 },
                new() { Id = 2, Email = "test", Password = "test", Role = UserRole.User, Name = "test", Salary = 30000 }
            };
            modelBuilder.Entity<User>().HasData(users);

#if DEBUG
            var workPeriods = new List<WorkPeriod>();
            var startDate = DateTime.UtcNow.AddMonths(-3);
            var endDate = DateTime.UtcNow;

            var random = new Random();

            foreach (var user in users)
            {
                DateTime userStartDate = startDate;

                int userId = user.Id;
                while (userStartDate < endDate)
                {
                    if (userStartDate.DayOfWeek != DayOfWeek.Saturday && userStartDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        int numWorkPeriods = random.Next(1, 4);

                        for (int i = 0; i < numWorkPeriods; i++)
                        {
                            TimeSpan startTime = TimeSpan.FromHours(random.Next(8, 12));
                            TimeSpan endTime = TimeSpan.FromHours(random.Next(13, 18));

                            workPeriods.Add(new WorkPeriod
                            {
                                Id = workPeriods.Count + 1,
                                StartAt = userStartDate.Add(startTime),
                                EndAt = userStartDate.Add(endTime),
                                UserId = userId
                            });
                        }
                    }
                    userStartDate = userStartDate.AddDays(1);
                }
            }

            modelBuilder.Entity<WorkPeriod>().HasData(workPeriods);
#endif

            base.OnModelCreating(modelBuilder);
        }
    }
}
