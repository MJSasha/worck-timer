﻿using QuickActions.Api;
using WorkTimer.Common.Models;

namespace WorckTimer.Api.Repository
{
    public class WorkPeriodRepository : CrudRepository<WorkPeriod>
    {
        public WorkPeriodRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
