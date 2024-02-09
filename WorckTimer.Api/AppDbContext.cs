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
            modelBuilder.Entity<User>().HasData(new User { Id = 1, Email = "admin", Password = "admin", Role = UserRole.Admin, Name = "admin", Salary = 150000 });

            base.OnModelCreating(modelBuilder);
        }
    }
}
