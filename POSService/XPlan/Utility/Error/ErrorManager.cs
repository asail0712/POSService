using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.Utility.Error
{
    public static class CommonErrorCode
    {
        public const int InvalidRequest     = 800001;
        public const int NotFound           = 800002;
        public const int OperationFailed    = 800003;
        public const int Unauthorized       = 800004;
        public const int Forbidden          = 800005;
    }

    public class ErrorManager : IErrorManager
    {
        private int BuildErrorCode(int prefix, int subCode)
        {
            int errorCode = prefix * 1000 + subCode;

            if (errorCode < 100001 || errorCode > 799999)
            {
                throw new AppException(CommonErrorCode.InvalidRequest, "Business error code out of range.");
            }

            return errorCode;
        }

        public void ThrowInvalidRequest(int prefix, string message = null)
        {
            throw new AppException(BuildErrorCode(prefix, 1), message ?? "Invalid request.");
        }

        public void ThrowNotFound(int prefix, string message = null)
        {
            throw new AppException(BuildErrorCode(prefix, 2), message ?? "Resource not found.");
        }

        public void ThrowOperationFailed(int prefix, string message = null)
        {
            throw new AppException(BuildErrorCode(prefix, 3), message ?? "Operation failed.");
        }
    }
}
