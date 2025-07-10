using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace XPlan.Exceptions
{
    public class CustomException : Exception
    {
        public object CustomErrorDto { get; }

        public CustomException(string message, object customErrorDto) 
            : base(message)
        {
            CustomErrorDto = customErrorDto;
        }
    }

    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is CustomException ex)
            {
                // 回傳 400 Bad Request，Body 是你的 CustomErrorDto
                context.Result = new BadRequestObjectResult(ex.CustomErrorDto);
            }
            else
            {
                // 500 內部錯誤，回傳 ProblemDetails 格式
                context.Result = new ObjectResult(new ProblemDetails
                {
                    Title   = "API Internal Error",
                    Detail  = context.Exception.Message,
                    Status  = StatusCodes.Status500InternalServerError
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            // 表示這個錯誤已被處理，不要往上拋
            context.ExceptionHandled = true;

            return Task.CompletedTask;
        }
    }
}
