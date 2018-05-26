
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CartApi.Common
{
    public class RequireUserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userContext = context.HttpContext.RequestServices.GetService(typeof(IUserDataContext)) as IUserDataContext;
            if (userContext == null)
            {
                // no user - bad request
                context.Result = new BadRequestResult();
            }
            else if (userContext.CurrentUser == null)
            {
                // no user set - token validation failed
                context.Result = new UnauthorizedResult();
            }
        }
    }
}