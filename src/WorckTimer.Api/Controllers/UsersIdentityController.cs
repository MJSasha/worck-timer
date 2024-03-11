using Microsoft.AspNetCore.Mvc;
using QuickActions.Api.Identity.Controllers;
using QuickActions.Api.Identity.Services;
using QuickActions.Common.Data;
using QuickActions.Common.Specifications;
using System.Net;
using System.Web.Http;
using WorkTimer.Api.Repository;
using WorkTimer.Api.Utils;
using WorkTimer.Common.Data;
using WorkTimer.Common.Interfaces;
using WorkTimer.Common.Models;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace WorkTimer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersIdentityController : IdentityController<User>, IUsersIdentity
    {
        private readonly UsersRepository usersRepository;

        public UsersIdentityController(SessionsService<User> sessionsService, UsersRepository usersRepository) : base(
            sessionsService)
        {
            this.usersRepository = usersRepository;
        }

        [HttpPost("login")]
        public async Task<string> Login(AuthModel authModel)
        {
            if (string.IsNullOrWhiteSpace(authModel.Email) || string.IsNullOrWhiteSpace(authModel.Password))
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var user = await usersRepository.Read(new Specification<User>(u =>
                u.Email == authModel.Email && u.Credentials.Password == authModel.Password.GetHash()));
            if (user == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "User not found." });
            }

            return sessionsService.CreateSession(new Session<User> { Data = user });
        }
    }
}