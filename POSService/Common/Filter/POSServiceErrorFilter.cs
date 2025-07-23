using Microsoft.AspNetCore.Http;

using Common.Exceptions;

using XPlan.Utility.Exceptions;

namespace Common.Filter
{
    public class POSServiceErrorCode
    {
        // Product
        public const int ProductNotFound            = 100001;
        public const int InvalidProductItem         = 100002;
        public const int ProductStockReduceFailed   = 100003;
        public const int ProductDatabaseOperation   = 100004;

        // Order
        public const int OrderNotFound              = 200001;
        public const int InvalidOrderProduct        = 200002;
        public const int OrderStatusUpdate          = 200003;
        public const int OrderDatabaseOperation     = 200004;

        // OrderRecall
        public const int OrderRecallNotFound            = 300001;
        public const int InvalidOrderRecallRequest      = 300002;
        public const int OrderRecallDatabaseOperation   = 300003;
        public const int OrderRecallAggregation         = 300004;

        // Dish
        public const int DishItemNotFound               = 400001;
        public const int DishItemOutOfStock             = 400002;
        public const int InvalidDishItemOperation       = 400003;
        public const int DishItemDatabaseOperation      = 400004;

        // Management
        public const int StaffNotFound              = 500001;
        public const int InvalidPassword            = 500002;
        public const int InvalidChangePassword      = 500003;
        public const int DatabaseOperation          = 500004;
    }

    public class POSServiceErrorFilter : GlobalExceptionFilter
    {
        protected override CustomErrorResponse FilterOtherError(CustomException customException)
        {
            var response = new CustomErrorResponse();
         
            switch (customException)
            {
                case StaffNotFoundException exStaffNotFound:
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.ErrorCode  = POSServiceErrorCode.StaffNotFound;
                    response.Message    = "Staff account not found.";
                    response.Detail     = exStaffNotFound.Message;
                    return response;

                case InvalidStaffPasswordException exInvalidPwd:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.ErrorCode  = POSServiceErrorCode.InvalidPassword;
                    response.Message    = "Incorrect password.";
                    response.Detail     = exInvalidPwd.Message;
                    return response;

                case InvalidChangePasswordRequestException exInvalidChangeReq:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.ErrorCode  = POSServiceErrorCode.InvalidChangePassword;
                    response.Message    = "Invalid change password request.";
                    response.Detail     = exInvalidChangeReq.Message;
                    return response;

                case ManagementDatabaseOperationException exMgmtDb:
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.ErrorCode  = POSServiceErrorCode.DatabaseOperation;
                    response.Message    = "Management database operation failed.";
                    response.Detail     = exMgmtDb.Message;
                    return response;

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

                // 🍱 DishItemService exceptions
                case DishItemNotFoundException exDishNotFound:
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.ErrorCode  = POSServiceErrorCode.DishItemNotFound;
                    response.Message    = "Dish item not found.";
                    response.Detail     = exDishNotFound.Message;
                    return response;

                case DishItemOutOfStockException exOutOfStock:
                    response.StatusCode = StatusCodes.Status409Conflict;
                    response.ErrorCode  = POSServiceErrorCode.DishItemOutOfStock;
                    response.Message    = "Dish item out of stock.";
                    response.Detail     = exOutOfStock.Message;
                    return response;

                case InvalidDishItemOperationException exInvalidDishOp:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.ErrorCode  = POSServiceErrorCode.InvalidDishItemOperation;
                    response.Message    = "Invalid dish item operation.";
                    response.Detail     = exInvalidDishOp.Message;
                    return response;

                case DishItemDatabaseOperationException exDishDb:
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.ErrorCode  = POSServiceErrorCode.DishItemDatabaseOperation;
                    response.Message    = "Dish item database operation failed.";
                    response.Detail     = exDishDb.Message;
                    return response;

                default:
                    // 沒有命中，讓 GlobalExceptionFilter 處理
                    return base.FilterOtherError(customException);
            }
        }
    }
}
