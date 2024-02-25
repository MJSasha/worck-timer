using QuickActions.Common.Data;
using Refit;

namespace QuickActions.Common.Interfaces
{
    public interface IIdentity<T>
    {
        [Post("/logout")]
        public Task Logout();
        [Post("/authenticate")]
        public Task<Session<T>> Authenticate();
    }
}
