using XPlan.Utility.Filter;

namespace Common.Filter
{
    public class ControllerAddSummaryFilter : AddSummaryFilter
    {
        protected override Dictionary<string, string> GetSummaryInfo()
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>()
            {
                // 額外
                { "SalesController.GetTotalSalesAmount", "取得銷售總金額" },
                { "SalesController.GetConsumptionCount", "取得產品消費次數" },
                { "OrderController.ModifyOrderStatus", "修改訂單狀態" },
            
                // 基本
                { "SalesController.Create", "新增銷售資料" },
                { "SalesController.GetAll", "取得所有銷售資料" },
                { "SalesController.GetById", "依據 ID 取得銷售資料" },
                { "SalesController.Update", "更新銷售資料" },
                { "SalesController.Delete", "刪除銷售資料" },

                { "ProductController.Create", "新增產品資料" },
                { "ProductController.GetAll", "取得所有產品資料" },
                { "ProductController.GetById", "依據 ID 取得產品資料" },
                { "ProductController.Update", "更新產品資料" },
                { "ProductController.Delete", "刪除產品資料" },

                { "OrderController.Create", "新增訂單資料" },
                { "OrderController.GetAll", "取得所有訂單資料" },
                { "OrderController.GetById", "依據 ID 取得訂單資料" },
                { "OrderController.Update", "更新訂單資料" },
                { "OrderController.Delete", "刪除訂單資料" },

                { "MenuItemController.Create", "新增單品資料" },
                { "MenuItemController.GetAll", "取得所有單品資料" },
                { "MenuItemController.GetById", "依據 ID 取得單品資料" },
                { "MenuItemController.Update", "更新單品資料" },
                { "MenuItemController.Delete", "刪除單品資料" },

                { "ManagementController.Create", "新增後台人員資料" },
                { "ManagementController.GetAll", "取得所有後台人員資料" },
                { "ManagementController.GetById", "依據 ID 取得後台人員資料" },
                { "ManagementController.Update", "更新後台人員資料" },
                { "ManagementController.Delete", "刪除後台人員資料" },
            };

            return keyValuePairs;
        }
    }
}
