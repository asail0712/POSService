using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace XPlan.Utility.Filter
{
    // 用於 Swagger 文件中為特定 API 加上授權標記的 OperationFilter
    public class AuthorizeCheckFilter : IOperationFilter
    {
        // 可覆寫取得需要授權的 API 清單（格式為 ControllerName.MethodName）
        protected virtual HashSet<string> GetAuthorizedApi()
        {
            return new HashSet<string>();
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authorizedMethods   = GetAuthorizedApi();
            var controllerName      = context.MethodInfo.ReflectedType!.Name;   // 取得 Controller 名稱 (例如 UserController)
            var methodName          = context.MethodInfo.Name;                  // 取得方法名稱 (例如 GetUser)
            var fullMethodName      = $"{controllerName}.{methodName}";         // 組合完整方法名稱字串

            // 若此 API 需要授權，則設定 SecurityRequirement
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

                // 加入 Bearer 權限需求 (無 scopes)
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [scheme] = new string[] { }
                });
            }
        }
    }
}
