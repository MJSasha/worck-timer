using Microsoft.AspNetCore.Mvc;
using QuickActions.Api;
using QuickActions.Api.Identity.IdentityCheck;
using WorkTimer.Api.Repository;
using WorkTimer.Common.Interfaces;
using WorkTimer.Common.Models;

namespace WorkTimer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [IdentityAll]
    public class UsersController : CrudController<User>, IUsers
    {
        public UsersController(UsersRepository usersRepository) : base(usersRepository)
        {
        }
    }
}