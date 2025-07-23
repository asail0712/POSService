using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extens
{
    public static class SwaggerExtension
    {
        static public void GenerateSwaggerDoc(this SwaggerGenOptions c, IWebHostEnvironment env)
        {
            // === Backpage 分頁 ===
            c.SwaggerDoc("BackStage", new OpenApiInfo
            {
                Title = "POS Service BackStage API",
                Version = "v1",
                Description = $"現在環境：{env.EnvironmentName}"
            });

            // === App 分頁 ===
            c.SwaggerDoc("App", new OpenApiInfo
            {
                Title       = "POS Service API",
                Version     = "v1",
                Description = $"現在環境：{env.EnvironmentName}"
            });

            // === Debug 分頁 ===
            c.SwaggerDoc("Debug", new OpenApiInfo
            {
                Title       = "POS Service Debug",
                Version     = "v1",
                Description = $"現在環境：{env.EnvironmentName}"
            });

            c.DocInclusionPredicate((docName, apiDesc) =>
            {
                // 先確定能取到 MethodInfo（方法資訊）
                if (!apiDesc.TryGetMethodInfo(out var methodInfo))
                {
                    return false;
                }
                
                string derivedClassName = methodInfo.ReflectedType!.Name;       // 取得「衍生類別名稱」= 方法所在的類別名稱
                string methodName       = methodInfo.Name;                      // 取得「函數名稱」
                string combinedName     = $"{derivedClassName}.{methodName}";   // 合併

                List<string> apiInAPP = new List<string>
                {
                    "ProductController.GetAllBrief",
                    "ProductController.GetBrief"
                };

                if(docName == "App" && apiInAPP.Contains(combinedName))
                {
                    return true;
                }
                else if(docName == "BackStage" && !apiInAPP.Contains(combinedName))
                {
                    return true;
                }

                return false;
            });
        }

        static public void SwaggerEndpoints(this SwaggerUIOptions c)
        {
            c.SwaggerEndpoint("/swagger/BackStage/swagger.json", "POS 後台 API");
            c.SwaggerEndpoint("/swagger/App/swagger.json", "POS 前端 API");
            c.SwaggerEndpoint("/swagger/Debug/swagger.json", "Debug API");
        }
    }
}
