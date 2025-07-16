using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.Utility
{
    public static class ApplicationBuilder
    {
        public static IApplicationBuilder UseJwtPathAuth(this IApplicationBuilder builder)
        {
            // JWT 驗證 這裡的順序很重要，必須先認證身分，再授權權限   
            builder.UseAuthentication();              // 先認證身分
            builder.UseAuthorization();               // 再授權權限
            return builder;
        }
    }
}
