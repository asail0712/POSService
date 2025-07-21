using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.Utility.Filter
{
    public class AuthorizeCheckFilter : IOperationFilter
    {
        protected virtual HashSet<string> GetAuthorizedApi()
        {
            // override
            return new HashSet<string>();
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authorizedMethods   = GetAuthorizedApi();
            var controllerName      = context.MethodInfo.ReflectedType.Name; // e.g. UserController
            var methodName          = context.MethodInfo.Name;                   // e.g. GetUser
            var fullMethodName      = $"{controllerName}.{methodName}";

            if (authorizedMethods.Contains(fullMethodName))
            {
                if (operation.Security == null)
                {
                    operation.Security = new List<OpenApiSecurityRequirement>();
                }

                var scheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                };

                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [scheme] = new string[] { }
                });
            }
        }
    }
}
