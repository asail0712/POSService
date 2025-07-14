using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.Utility.Error
{
    public interface IErrorManager
    {
        void ThrowInvalidRequest(int prefix, string message = null);
        void ThrowNotFound(int prefix, string message = null);
        void ThrowOperationFailed(int prefix, string message = null);
    }
}
