using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.Utility.Filter
{
    public class HiddenApiDocumentFilter : IDocumentFilter
    {
        protected virtual HashSet<string> GetHiddenList()
        {
            // override
            return new HashSet<string>();
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var closedApis      = GetHiddenList();
            var pathsToRemove   = new List<string>();

            foreach (var path in swaggerDoc.Paths)
            {
                var operationsToRemove = new List<OperationType>();

                foreach (var operation in path.Value.Operations)
                {
                    // 找出這個 API 的描述
                    var apiDesc = context.ApiDescriptions.FirstOrDefault(d =>
                        d.RelativePath.Equals(path.Key.TrimStart('/'), StringComparison.OrdinalIgnoreCase)
                        && d.HttpMethod.Equals(operation.Key.ToString(), StringComparison.OrdinalIgnoreCase));

                    if (apiDesc != null)
                    {                        
                        var controllerName  = apiDesc.ActionDescriptor.RouteValues["controller"] + "Controller";            // Controller 名稱（例如 UserController）
                        var methodName      = (apiDesc.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo.Name;    // Method 名稱（例如 GetUser）
                        var fullMethodName  = $"{controllerName}.{methodName}";

                        if (closedApis.Contains(fullMethodName))
                        {
                            operationsToRemove.Add(operation.Key);
                        }
                    }
                }

                // 移除該路徑下被隱藏的所有 Operation
                foreach (var op in operationsToRemove)
                {
                    path.Value.Operations.Remove(op);
                }

                // 如果該路徑已無任何 Operation，則整條路徑移除
                if (!path.Value.Operations.Any())
                {
                    pathsToRemove.Add(path.Key);
                }
            }

            // 移除路徑
            foreach (var pathKey in pathsToRemove)
            {
                swaggerDoc.Paths.Remove(pathKey);
            }
        }
    }
}
