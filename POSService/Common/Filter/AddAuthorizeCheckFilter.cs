using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Utility.Filter;

namespace Common.Filter
{
    public class AddAuthorizeCheckFilter : AuthorizeCheckFilter
    {
        protected override HashSet<string> GetAuthorizedApi()
        {
            HashSet<string> hiddenSet = new HashSet<string>()
            {
                "ManagementController.ChangePassword",
                "ManagementController.Create",
                "ManagementController.GetAll",
                "ManagementController.Get",
                "ManagementController.ChangeData",
                "ManagementController.Delete"
            };

            return hiddenSet;
        }
    }
}
