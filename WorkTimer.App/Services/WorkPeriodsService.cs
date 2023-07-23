using QuickActions.Common.Specifications;
using WorkTimer.Common.Interfaces;
using WorkTimer.Common.Models;

namespace WorkTimer.App.Services
{
    public class WorkPeriodsService
    {
        private readonly IWorkPeriod workPeriodService;
        private readonly IUsersIdentity usersIdentityService;
        private readonly ExceptionsHandler exceptionsHandler;

        public WorkPeriodsService(IWorkPeriod workPeriodService, IUsersIdentity usersIdentityService, ExceptionsHandler exceptionsHandler)
        {
            this.workPeriodService = workPeriodService;
            this.usersIdentityService = usersIdentityService;
            this.exceptionsHandler = exceptionsHandler;
        }

        public async Task<WorkPeriod> StartPeriod()
        {
            WorkPeriod period = null;
            try
            {
                var currentUser = (await usersIdentityService.Authenticate()).Data;
                period = new WorkPeriod
                {
                    StartAt = DateTime.UtcNow,
                    UserId = currentUser.Id,
                };

                await workPeriodService.Create(period);
            }
            catch (Exception ex)
            {
                await exceptionsHandler.Handle(ex);
            }
            return period;
        }

        public async Task CompletePeriod()
        {
            try
            {
                var currentUser = (await usersIdentityService.Authenticate()).Data;
                var currentPeriod = await workPeriodService.Read(new Specification<WorkPeriod>(wp => wp.UserId == currentUser.Id && wp.EndAt == null));
                if (currentUser == null) return;

                currentPeriod.EndAt = DateTime.UtcNow;
                await workPeriodService.Update(currentPeriod);
            }
            catch (Exception ex)
            {
                await exceptionsHandler.Handle(ex);
            }
        }

        public async Task<WorkPeriod> LoadCurrentPeriod()
        {
            WorkPeriod period = null;
            try
            {
                var currentUser = (await usersIdentityService.Authenticate()).Data;
                period = await workPeriodService.Read(new Specification<WorkPeriod>(wp => wp.UserId == currentUser.Id && wp.EndAt == null));
            }
            catch (Exception ex)
            {
                await exceptionsHandler.Handle(ex);
            }
            return period;
        }

        public async Task<List<WorkPeriod>> LoadPeriods(DateTime startAfter, DateTime? endBefore = null)
        {
            try
            {
                var currentUser = (await usersIdentityService.Authenticate()).Data;
                if (endBefore != null) return await workPeriodService.Read(new Specification<WorkPeriod>(wp => wp.StartAt >= startAfter && wp.EndAt <= endBefore && wp.UserId == currentUser.Id), 0, int.MaxValue);
                else return await workPeriodService.Read(new Specification<WorkPeriod>(wp => wp.StartAt >= startAfter && wp.EndAt != null && wp.UserId == currentUser.Id), 0, int.MaxValue);

            }
            catch (Exception ex)
            {
                await exceptionsHandler.Handle(ex);
                return new List<WorkPeriod>();
            }
        }
    }
}