﻿using Microsoft.EntityFrameworkCore;
using WorkTimer.Common.Models;

namespace WorkTimer.Api
{
    public class AppDbContext : DbContext
    {
        public DbSet<WorkPeriod> WorkPeriods { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
