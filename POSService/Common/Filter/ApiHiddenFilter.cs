using XPlan.Utility.Filter;

namespace Common.Filter
{
    public class ApiHiddenFilter : HiddenApiDocumentFilter
    {
        protected override HashSet<string> GetHiddenList()
        {
            HashSet<string> hiddenSet = new HashSet<string>()
            {
                "OrderController.DeleteAsync",
                "OrderRecallController.CreateAsync",
                "OrderRecallController.GetAllAsync",
                "OrderRecallController.GetAsync",
                "OrderRecallController.UpdateAsync",
                "OrderRecallController.DeleteAsync",
                "ManagementController.UpdateAsync"
            };

            return hiddenSet;
        }
    }
}
