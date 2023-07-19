using Microsoft.EntityFrameworkCore;
using QuickActions.Api;
using WorkTimer.Common.Models;

namespace WorkTimer.Api.Repository
{
    public class UsersRepository : CrudRepository<User>
    {
        public UsersRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
