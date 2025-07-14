using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.Utility.Error
{
    public class AppException : Exception
    {
        public int ErrorCode { get; }

        public AppException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
