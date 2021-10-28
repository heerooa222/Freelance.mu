using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Freelancer.mu.Models
{
    public class AuthenticationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var session = context.HttpContext.Session.GetString("user");
            if (session == null)
            {
                context.Result = new RedirectToRouteResult
                (
                    new RouteValueDictionary(new
                    {
                        action = "login",
                        controller = "account",
                        mustlogin = "true",
                        next = $"{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString.Value}"
                    })
                );
            }
        }
    }
}