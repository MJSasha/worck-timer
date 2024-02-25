using QuickActions.Common.Interfaces;
using Refit;
using WorkTimer.Common.Data;
using WorkTimer.Common.Models;

namespace WorkTimer.Common.Interfaces
{
    public interface IUsersIdentity : IIdentity<User>
    {
        [Post("/login")]
        public Task<string> Login(AuthModel authModel);
    }
}
