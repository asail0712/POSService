using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
