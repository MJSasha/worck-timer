using QuickActions.Api.Identity.Controllers;
using QuickActions.Api.Identity.Services;
using WorkTimer.Common.Models;

namespace WorkTimer.Api.Controllers
{
    public class UsersIdentityController : IdentityController<User>
    {
        public UsersIdentityController(SessionsService<User> sessionsService) : base(sessionsService)
        {
        }
    }
}
