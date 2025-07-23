using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace XPlan.Utility.Exceptions
{
    public class CustomException : Exception
    {
        public CustomException(string message)
            : base(message)
        { }

        public CustomException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }

    public class EntityNotFoundException : CustomException
    {
        public EntityNotFoundException(string entityName, string key)
            : base($"{entityName} with key '{key}' was not found.") { }
    }

    public class CacheMissException : CustomException
    {
        public CacheMissException(string cacheKey)
            : base($"Cache miss for key '{cacheKey}'.") { }
    }

    public class InvalidEntityException : CustomException
    {
        public InvalidEntityException(string entityName)
            : base($"Invalid or null entity of type '{entityName}' encountered.")
        { }
    }

    public class DatabaseOperationException : CustomException
    {
        public DatabaseOperationException(string operation, string entityName, Exception inner)
            : base($"Database operation '{operation}' failed for entity '{entityName}'. Becuz {inner.Message}", inner) { }
    }

    public class InvalidRepositoryArgumentException : CustomException
    {
        public InvalidRepositoryArgumentException(string parameterName, string reason)
            : base($"Invalid argument '{parameterName}': {reason}.")
        { }
    }
    public class CustomErrorCode
    {
        public const int EntityNotFound             = 800001;
        public const int CacheMiss                  = 800002;
        public const int InvalidEntity              = 800003;
        public const int DatabaseOperation          = 800004;
        public const int InvalidRepositoryArgument  = 800005;
        public const int UnknowErrorHappen          = 800006;
    }

    public class CustomErrorResponse
    {
        public int ErrorCode { get; set; }  = CustomErrorCode.UnknowErrorHappen;
        public int StatusCode { get; set; } = StatusCodes.Status500InternalServerError;
        public string Message { get; set; } = "An unexpected error occurred.";
        public string Detail { get; set; }  = "";
        public CustomErrorResponse() { }
        public CustomErrorResponse(Exception exception) 
        {
            ErrorCode   = CustomErrorCode.UnknowErrorHappen;
            StatusCode  = 500;
            Message     = "An unexpected error occurred.";
            Detail      = exception.Message;
        }
    }

    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        protected virtual CustomErrorResponse FilterOtherError(CustomException customException)
        {
            // for override
            return new CustomErrorResponse(customException);
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            var exception   = context.Exception;
            var response    = new CustomErrorResponse(context.Exception);

            // 🎯 根據 Exception 類型決定 HTTP 狀態碼 & 訊息
            switch (exception)
            {
                case InvalidEntityException entEx:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.ErrorCode  = CustomErrorCode.InvalidEntity;
                    response.Message    = "Invalid request parameter.";
                    response.Detail     = entEx.Message;
                    break;

                case InvalidRepositoryArgumentException argEx:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.ErrorCode  = CustomErrorCode.InvalidRepositoryArgument;
                    response.Message    = "Invalid request parameter.";
                    response.Detail     = argEx.Message;
                    break;

                case EntityNotFoundException notFoundEx:
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.ErrorCode  = CustomErrorCode.EntityNotFound;
                    response.Message    = "Resource not found.";
                    response.Detail     = notFoundEx.Message;
                    break;

                case CacheMissException cacheEx:
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.ErrorCode  = CustomErrorCode.CacheMiss;
                    response.Message    = "Cache data not found.";
                    response.Detail     = cacheEx.Message;
                    break;

                case DatabaseOperationException dbEx:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.ErrorCode  = CustomErrorCode.DatabaseOperation;
                    response.Message    = "Database operation failed.";
                    response.Detail     = dbEx.Message;
                    break;

                case CustomException otherEx:
                    // 捕捉其他 CustomException 子類別
                    response = FilterOtherError(otherEx);
                    break;
                default:
                    // 未知錯誤
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.ErrorCode  = 500;
                    response.Message    = "Internal Server Error";
                    response.Detail     = exception.Message;
                    break;
            }

            context.Result = new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };

            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
