using Common.DTO.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Utility.Exceptions;

namespace Common.Exceptions
{
    // Management
    public class StaffNotFoundException : CustomException
    {
        public StaffNotFoundException(string account)
            : base($"Staff with account '{account}' was not found.") { }
    }

    public class InvalidStaffPasswordException : CustomException
    {
        public InvalidStaffPasswordException()
            : base("The provided password is incorrect.") { }
    }

    public class InvalidChangePasswordRequestException : CustomException
    {
        public InvalidChangePasswordRequestException()
            : base("The change password request is invalid. Please check the provided data.") { }
    }

    public class ManagementDatabaseOperationException : CustomException
    {
        public ManagementDatabaseOperationException(string operation, Exception inner)
            : base($"Management database operation '{operation}' failed.", inner) { }
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

    // Dish
    public class DishItemNotFoundException : CustomException
    {
        public DishItemNotFoundException(string id)
            : base($"Dish item with ID '{id}' was not found.") { }
    }

    public class InvalidDishItemOperationException : CustomException
    {
        public InvalidDishItemOperationException(string reason)
            : base($"Invalid operation on dish item: {reason}") { }
    }

    public class DishItemOutOfStockException : CustomException
    {
        public DishItemOutOfStockException(string id, int requested, int available)
            : base($"Dish item '{id}' out of stock. Requested: {requested}, Available: {available}") { }
    }

    public class DishItemDatabaseOperationException : CustomException
    {
        public DishItemDatabaseOperationException(string operation, Exception inner)
            : base($"Database operation '{operation}' failed in DishItemService. Because: {inner.Message}", inner) { }
    }
}
