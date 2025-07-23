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
                case OrderNotFoundException notFoundEx:
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.ErrorCode  = POSServiceErrorCode.OrderNotFound;
                    response.Message    = "Order not found.";
                    response.Detail     = notFoundEx.Message;
                    return response;

                case InvalidOrderProductException invalidProductEx:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.ErrorCode  = POSServiceErrorCode.InvalidOrderProduct;
                    response.Message    = "Invalid product in order.";
                    response.Detail     = invalidProductEx.Message;
                    return response;

                case OrderStatusUpdateException statusEx:
                    response.StatusCode = StatusCodes.Status409Conflict;
                    response.ErrorCode  = POSServiceErrorCode.OrderStatusUpdate;
                    response.Message    = "Failed to update order status.";
                    response.Detail     = statusEx.Message;
                    return response;

                case OrderDatabaseOperationException dbEx:
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.ErrorCode  = POSServiceErrorCode.OrderDatabaseOperation;
                    response.Message    = "Order database operation failed.";
                    response.Detail     = dbEx.Message;
                    return response;

                case OrderRecallNotFoundException notFoundEx:
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.ErrorCode  = POSServiceErrorCode.OrderRecallNotFound;
                    response.Message    = "Order recall data not found.";
                    response.Detail     = notFoundEx.Message;
                    return response;

                case InvalidOrderRecallRequestException invalidReqEx:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.ErrorCode  = POSServiceErrorCode.InvalidOrderRecallRequest;
                    response.Message    = "Invalid order recall request.";
                    response.Detail     = invalidReqEx.Message;
                    return response;

                case OrderRecallAggregationException aggEx:
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.ErrorCode  = POSServiceErrorCode.OrderRecallAggregation;
                    response.Message    = "Failed to aggregate order recall data.";
                    response.Detail     = aggEx.Message;
                    return response;

                case OrderRecallDatabaseOperationException dbEx:
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.ErrorCode  = POSServiceErrorCode.OrderRecallDatabaseOperation;
                    response.Message    = "Order recall database operation failed.";
                    response.Detail     = dbEx.Message;
                    return response;

                default:
                    // 沒有命中，讓 GlobalExceptionFilter 處理
                    return base.FilterOtherError(customException);
            }
        }
    }
}
