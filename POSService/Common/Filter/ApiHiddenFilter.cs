using XPlan.Utility.Filter;

namespace Common.Filter
{
    public class ApiHiddenFilter : HiddenApiDocumentFilter
    {
        protected override HashSet<string> GetHiddenList()
        {
            HashSet<string> hiddenSet = new HashSet<string>()
            {
                "OrderController.Delete",
                "OrderRecallController.Create",
                "OrderRecallController.GetAll",
                "OrderRecallController.Get",
                "OrderRecallController.Update",
                "OrderRecallController.Delete",
                "ManagementController.Update"
            };

            return hiddenSet;
        }
    }
}
