using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using QuickActions.Api.Identity.Services;
using QuickActions.Common.Exceptions;
using System.Net;
using System.Reflection;

namespace QuickActions.Api.Identity.IdentityCheck
{
    public class IdentityFilter<T> : IActionFilter
    {
        private readonly SessionsService<T> sessionsService;

        public IdentityFilter(SessionsService<T> sessionsService)
        {
            this.sessionsService = sessionsService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller.GetType();
            var actionName = (context.ActionDescriptor as ControllerActionDescriptor)?.ActionName;

            var identityAll = (IdentityAllAttribute)controller.GetCustomAttributes(typeof(IdentityAllAttribute), true).FirstOrDefault();

            bool isIdentityMatched;
            if (identityAll == null)
            {
                MethodBase method = controller.GetMethod(actionName);
                var identity = (IdentityAttribute)method.GetCustomAttributes(typeof(IdentityAttribute), true).FirstOrDefault();
                if (identity == null) return;
                isIdentityMatched = sessionsService.CheckAccess(identity.RoleNames);
            }
            else
            {
                isIdentityMatched = sessionsService.CheckAccess(identityAll.RoleNames);
            }

            if (!isIdentityMatched)
            {
                throw new ResponseException(HttpStatusCode.Forbidden);
            }
        }
    }
}