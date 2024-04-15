using QuickActions.Api;
using WorkTimer.Api.Utils;
using WorkTimer.Common.Models;

namespace WorkTimer.Api.Repository
{
    public class UsersRepository : CrudRepository<User>
    {
        public UsersRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public override Task Create(User entity)
        {
            if (entity.Credentials != null) entity.Credentials.Password = entity.Credentials.Password.GetHash();
            return base.Create(entity);
        }

        public override Task Update(User entity)
        {
            if (entity.Credentials != null) entity.Credentials.Password = entity.Credentials.Password.GetHash();
            return base.Update(entity);
        }
    }
}