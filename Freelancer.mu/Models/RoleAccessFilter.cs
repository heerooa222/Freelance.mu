using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Services.Freelancer.Models;
using Services.Freelancer.Utility;

namespace Freelancer.mu.Models
{
    public class RoleAccessFilter: Attribute, IAuthorizationFilter
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
                        next = context.HttpContext.Request.Path.Value
                    })
                );
            }
            else
            {
                var userModel = JsonConvert.DeserializeObject<UserModel>(session);
                if (!userModel.UserType.TypeName.Equals(UserType.EMPLOYER))
                {
                    context.Result = new RedirectToRouteResult
                    (
                        new RouteValueDictionary(new
                        {
                            action = "code403",
                            controller = "error",
                            url = context.HttpContext.Request.Path.Value
                        })
                    );
                }
            }
        }
    }
}