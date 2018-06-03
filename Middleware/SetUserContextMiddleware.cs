
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using CartApi.Common;
using CartApi.Extensions;
using Microsoft.Extensions.Configuration;
using CartApi.Models;
using System;

namespace CartApi.Middleware
{
    public class SetUserContextMiddleware
    {
        private readonly RequestDelegate nextDelegate;
        private readonly IConfiguration configuration;

        public SetUserContextMiddleware(RequestDelegate del, IConfiguration config)
        {
            this.nextDelegate = del;
            this.configuration = config;
        }

        public async Task Invoke(HttpContext context)
        {
            // verify that the user supplied the Authorization Header
            if (context.Request.Headers.Keys.Contains("Authorization"))
            {
                string authorizationToken = context.Request.Headers["Authorization"];
                User registeredUser = await authorizationToken.ValidateToken();

                // add it to the request context
                IUserDataContext dataContext = (IUserDataContext)context.RequestServices.GetService(typeof(IUserDataContext));
                dataContext.SetCurrentUser(registeredUser);
            }

            await this.nextDelegate.Invoke(context);
        }
    }
}