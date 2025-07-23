using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Utility.Exceptions;

namespace Common.Filter
{
    public class POSServiceErrorFilter : GlobalExceptionFilter
    {
        protected override CustomErrorResponse FilterOtherError(CustomException customException, out int errorCode)
        {
            return base.FilterOtherError(customException, out errorCode);
        }
    }
}
