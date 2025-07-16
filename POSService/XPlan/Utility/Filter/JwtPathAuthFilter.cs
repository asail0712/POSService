using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.Utility.Filter
{
    public class ProtectedApi
    {
        public string Method { get; set; }  = "";
        public string Path { get; set; }    = "";
    }

    public class JwtPathAuthFilter : IAsyncResourceFilter
    {
        private readonly IConfiguration _config;
        private readonly List<ProtectedApi> _protectedPaths;

        public JwtPathAuthFilter(IConfiguration config)
        {
            _config         = config;
            _protectedPaths = _config.GetSection("ProtectedApis").Get<List<ProtectedApi>>() ?? new List<ProtectedApi>();
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var requestMethod = context.HttpContext.Request.Method;
            var requestPath = context.HttpContext.Request.Path.Value?.ToLower();

            bool needsAuth = _protectedPaths.Any(api =>
            {
                // Path 使用 Route 模板比對
                var routePattern = RoutePatternFactory.Parse(api.Path.ToLower());
                var matcher = new TemplateMatcher(
                    TemplateParser.Parse(api.Path.ToLower()),
                    new RouteValueDictionary()
                );

                bool pathMatches = matcher.TryMatch(requestPath, new RouteValueDictionary());

                // Method 不填則所有 Method 都驗證
                bool methodMatches = string.IsNullOrWhiteSpace(api.Method) ||
                                     api.Method.Equals(requestMethod, StringComparison.OrdinalIgnoreCase);

                return pathMatches && methodMatches;
            });

            if (needsAuth)
            {
                var user = context.HttpContext.User;

                if (!user.Identity?.IsAuthenticated ?? true)
                {
                    context.Result = new UnauthorizedResult(); // 回 401
                    return;
                }
            }

            await next();

            await next();
        }
    }
}
