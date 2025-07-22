using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.Utility.Exceptions
{
    public class RepositoryException : Exception
    {
        public RepositoryException(string message) : base(message) { }
        public RepositoryException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class EntityNotFoundException : RepositoryException
    {
        public EntityNotFoundException(string entityName, string key)
            : base($"{entityName} with key '{key}' was not found.") { }
    }

    public class CacheMissException : RepositoryException
    {
        public CacheMissException(string cacheKey)
            : base($"Cache miss for key '{cacheKey}'.") { }
    }

    public class InvalidEntityException : RepositoryException
    {
        public InvalidEntityException(string entityName)
            : base($"Invalid or null entity of type '{entityName}' encountered.") { }
    }

    public class DatabaseOperationException : RepositoryException
    {
        public DatabaseOperationException(string operation, string entityName, Exception inner)
            : base($"Database operation '{operation}' failed for entity '{entityName}'.", inner) { }
    }

    public class InvalidRepositoryArgumentException : RepositoryException
    {
        public InvalidRepositoryArgumentException(string parameterName, string reason)
            : base($"Invalid argument '{parameterName}': {reason}.") { }
    }
}
