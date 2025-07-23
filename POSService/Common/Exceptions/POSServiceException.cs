using Common.DTO.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Utility.Exceptions;

namespace Common.Exceptions
{
    public class POSServiceErrorCode
    {
        // Product
        public const int ProductNotFound                = 100001;
        public const int InvalidProductItem             = 100002;
        public const int ProductStockReduceFailed       = 100003;
        public const int ProductDatabaseOperation       = 100004;

        // Order
        public const int OrderNotFound                  = 200001;
        public const int InvalidOrderProduct            = 200002;
        public const int OrderStatusUpdate              = 200003;
        public const int OrderDatabaseOperation         = 200004;

        // OrderRecall
        public const int OrderRecallNotFound            = 300001;
        public const int InvalidOrderRecallRequest      = 300002;
        public const int OrderRecallDatabaseOperation   = 300003;
        public const int OrderRecallAggregation         = 300004;
    }


    // Product
    public class ProductNotFoundException : CustomException
    {
        public ProductNotFoundException(string key)
            : base($"Product with key '{key}' was not found.") { }
    }

    public class InvalidProductItemException : CustomException
    {
        public InvalidProductItemException(string message)
            : base($"Invalid product item: {message}") { }
    }

    public class ProductStockReduceFailedException : CustomException
    {
        public ProductStockReduceFailedException(string itemId)
            : base($"Failed to reduce stock for item '{itemId}'.") { }
    }

    public class ProductDatabaseOperationException : CustomException
    {
        public ProductDatabaseOperationException(string operation, Exception inner)
            : base($"Database operation '{operation}' failed in ProductService. Because: {inner.Message}", inner) { }
    }

    // Order
    public class OrderNotFoundException : CustomException
    {
        public OrderNotFoundException(string orderId)
            : base($"Order with ID '{orderId}' was not found.") { }
    }

    public class InvalidOrderProductException : CustomException
    {
        public InvalidOrderProductException(string message)
            : base($"Invalid product in order: {message}") { }
    }

    public class OrderDatabaseOperationException : CustomException
    {
        public OrderDatabaseOperationException(string operation, Exception inner)
            : base($"Database operation '{operation}' failed in OrderService. Because: {inner.Message}", inner) { }
    }

    public class OrderStatusUpdateException : CustomException
    {
        public OrderStatusUpdateException(string orderId, OrderStatus status)
            : base($"Failed to update status for order '{orderId}' to '{status}'.") { }
    }

    // OrderRecall
    public class OrderRecallNotFoundException : CustomException
    {
        public OrderRecallNotFoundException(string id)
            : base($"Order recall record with ID '{id}' was not found.") { }
    }

    public class InvalidOrderRecallRequestException : CustomException
    {
        public InvalidOrderRecallRequestException(string message)
            : base($"Invalid request in OrderRecallService: {message}") { }
    }

    public class OrderRecallDatabaseOperationException : CustomException
    {
        public OrderRecallDatabaseOperationException(string operation, Exception inner)
            : base($"Database operation '{operation}' failed in OrderRecallService. Because: {inner.Message}", inner) { }
    }

    public class OrderRecallAggregationException : CustomException
    {
        public OrderRecallAggregationException(string reason)
            : base($"Failed to aggregate sales data: {reason}") { }
    }
}
