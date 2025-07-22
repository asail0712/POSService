using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace XPlan.Utility.Exceptions
{
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception;

            // 🆕 統一回傳格式
            var errorResponse = new
            {
                code    = 500,
                message = "An unexpected error occurred.",
                details = exception.Message
            };

            int statusCode = StatusCodes.Status500InternalServerError;

            // 🎯 根據 Exception 類型決定 HTTP 狀態碼 & 訊息
            switch (exception)
            {
                case InvalidRepositoryArgumentException argEx:
                    statusCode      = StatusCodes.Status400BadRequest;
                    errorResponse   = new
                    {
                        code    = 400,
                        message = "Invalid request parameter.",
                        details = argEx.Message
                    };
                    break;

                case EntityNotFoundException notFoundEx:
                    statusCode      = StatusCodes.Status404NotFound;
                    errorResponse   = new
                    {
                        code    = 404,
                        message = "Resource not found.",
                        details = notFoundEx.Message
                    };
                    break;

                case CacheMissException cacheEx:
                    statusCode      = StatusCodes.Status404NotFound; // 或改 500 視你的需求
                    errorResponse   = new
                    {
                        code    = 404,
                        message = "Cache data not found.",
                        details = cacheEx.Message
                    };
                    break;

                case DatabaseOperationException dbEx:
                    statusCode      = StatusCodes.Status500InternalServerError;
                    errorResponse   = new
                    {
                        code    = 500,
                        message = "Database operation failed.",
                        details = dbEx.Message
                    };
                    break;

                case RepositoryException repoEx:
                    // 捕捉其他 RepositoryException 子類別
                    statusCode      = StatusCodes.Status500InternalServerError;
                    errorResponse   = new
                    {
                        code    = 500,
                        message = "Repository error.",
                        details = repoEx.Message
                    };
                    break;
                default:
                    // 未知錯誤
                    statusCode = StatusCodes.Status500InternalServerError;
                    errorResponse = new
                    {
                        code    = 500,
                        message = "Internal Server Error",
                        details = exception.Message
                    };
                    break;
            }

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
