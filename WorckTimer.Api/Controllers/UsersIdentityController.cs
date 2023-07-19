using Microsoft.AspNetCore.Mvc;
using QuickActions.Api.Identity.Controllers;
using QuickActions.Api.Identity.Services;
using QuickActions.Common.Data;
using QuickActions.Common.Specifications;
using System.Net;
using System.Web.Http;
using WorkTimer.Api.Data;
using WorkTimer.Api.Repository;
using WorkTimer.Common.Models;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace WorkTimer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersIdentityController : IdentityController<User>
    {
        private readonly SessionsService<User> sessionsService;
        private readonly UsersRepository usersRepository;

        public UsersIdentityController(SessionsService<User> sessionsService, UsersRepository usersRepository) : base(sessionsService)
        {
            this.sessionsService = sessionsService;
            this.usersRepository = usersRepository;
        }

        public async Task<string> Login(AuthModel authModel)
        {
            if (string.IsNullOrWhiteSpace(authModel.Email) || string.IsNullOrWhiteSpace(authModel.Password)) throw new HttpResponseException(HttpStatusCode.BadRequest);

            var user = await usersRepository.Read(new Specification<User>(u => u.Email == authModel.Email && u.Password == authModel.Password)) ?? throw new HttpResponseException(HttpStatusCode.Unauthorized);

            return sessionsService.CreateSession(new Session<User> { Data = user });
        }
    }
}
