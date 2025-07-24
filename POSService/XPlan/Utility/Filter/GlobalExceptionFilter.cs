using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace XPlan.Utility.Exceptions
{
    // 自訂基底例外類別，繼承自 System.Exception
    public class CustomException : Exception
    {
        public CustomException(string message)
            : base(message)
        { }

        public CustomException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }

    // 資料不存在例外
    public class EntityNotFoundException : CustomException
    {
        public EntityNotFoundException(string entityName, string key)
            : base($"{entityName} with key '{key}' was not found.") { }
    }

    // 快取未命中例外
    public class CacheMissException : CustomException
    {
        public CacheMissException(string cacheKey)
            : base($"Cache miss for key '{cacheKey}'.") { }
    }

    // 無效的實體例外
    public class InvalidEntityException : CustomException
    {
        public InvalidEntityException(string entityName)
            : base($"Invalid or null entity of type '{entityName}' encountered.")
        { }
    }

    // 資料庫操作失敗例外
    public class DatabaseOperationException : CustomException
    {
        public DatabaseOperationException(string operation, string entityName, Exception inner)
            : base($"Database operation '{operation}' failed for entity '{entityName}'. Becuz {inner.Message}", inner) { }
    }

    // 無效的儲存庫參數例外
    public class InvalidRepositoryArgumentException : CustomException
    {
        public InvalidRepositoryArgumentException(string parameterName, string reason)
            : base($"Invalid argument '{parameterName}': {reason}.")
        { }
    }

    // 自訂錯誤代碼常數
    public class CustomErrorCode
    {
        public const int EntityNotFound             = 800001;
        public const int CacheMiss                  = 800002;
        public const int InvalidEntity              = 800003;
        public const int DatabaseOperation          = 800004;
        public const int InvalidRepositoryArgument  = 800005;
        public const int UnknowErrorHappen          = 800006;
    }

    // 錯誤回應結構
    public class CustomErrorResponse
    {
        public int ErrorCode { get; set; }  = CustomErrorCode.UnknowErrorHappen;
        public int StatusCode { get; set; } = StatusCodes.Status500InternalServerError;
        public string Message { get; set; } = "An unexpected error occurred.";
        public string Detail { get; set; }  = "";

        public CustomErrorResponse() { }

        // 依 Exception 初始化錯誤訊息
        public CustomErrorResponse(Exception exception)
        {
            ErrorCode   = CustomErrorCode.UnknowErrorHappen;
            StatusCode  = 500;
            Message     = "An unexpected error occurred.";
            Detail      = exception.Message;
        }
    }

    // 全域例外攔截器，實作 IAsyncExceptionFilter
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        // 可覆寫用於處理其他自訂例外的邏輯
        protected virtual CustomErrorResponse FilterOtherError(CustomException customException)
        {
            return new CustomErrorResponse(customException);
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            var exception   = context.Exception;
            var response    = new CustomErrorResponse(context.Exception);

            // 根據例外類型設定回應狀態碼與訊息
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
                    // 處理其他自訂例外
                    response = FilterOtherError(otherEx);
                    break;

                default:
                    // 未知例外
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.ErrorCode  = 500;
                    response.Message    = "Internal Server Error";
                    response.Detail     = exception.Message;
                    break;
            }

            // 設定回傳結果與狀態碼
            context.Result = new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };

            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
