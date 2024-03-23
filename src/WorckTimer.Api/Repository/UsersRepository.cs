using QuickActions.Api;
using WorkTimer.Common.Models;

namespace WorkTimer.Api.Repository
{
    public class UsersRepository : CrudRepository<User>
    {
        public UsersRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}