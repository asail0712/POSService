using Microsoft.AspNetCore.Http;

using Common.Exceptions;

using XPlan.Utility.Exceptions;

namespace Common.Filter
{
    public class POSServiceErrorFilter : GlobalExceptionFilter
    {
        protected override CustomErrorResponse FilterOtherError(CustomException customException)
        {
            var response = new CustomErrorResponse();
         
            switch (customException)
            {
                case ProductNotFoundException notFoundEx:
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.ErrorCode  = POSServiceErrorCode.ProductNotFound;
                    response.Message    = "Product not found.";
                    response.Detail     = notFoundEx.Message;
                    return response;

                case InvalidProductItemException invalidItemEx:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.ErrorCode  = POSServiceErrorCode.InvalidProductItem;
                    response.Message    = "Invalid product item.";
                    response.Detail     = invalidItemEx.Message;
                    return response;

                case ProductStockReduceFailedException stockEx:
                    response.StatusCode = StatusCodes.Status409Conflict;
                    response.ErrorCode  = POSServiceErrorCode.ProductStockReduceFailed;
                    response.Message    = "Failed to reduce stock.";
                    response.Detail     = stockEx.Message;
                    return response;

                case ProductDatabaseOperationException dbOpEx:
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.ErrorCode  = POSServiceErrorCode.ProductDatabaseOperation;
                    response.Message    = "Database operation failed in ProductService.";
                    response.Detail     = dbOpEx.Message;
                    return response;

                default:
                    // 沒有命中，讓 GlobalExceptionFilter 處理
                    return base.FilterOtherError(customException);
            }
        }
    }
}
