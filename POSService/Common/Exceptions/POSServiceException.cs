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
        public const int ProductNotFound            = 100001;
        public const int InvalidProductItem         = 100002;
        public const int ProductStockReduceFailed   = 100003;
        public const int ProductDatabaseOperation   = 100004;
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
}
