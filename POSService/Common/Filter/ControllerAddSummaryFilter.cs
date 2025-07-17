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
                { "SalesController.GetSalesByTime", "取得銷售總金額" },
                { "SalesController.GetProductSalesByTime", "取得產品消費總金額" },
                { "OrderController.ModifyOrderStatus", "修改訂單狀態" },
                { "ProductController.GetBriefAsync", "取得產品簡介" },
                { "DishItemController.GetBriefAsync", "取得菜品簡介" },
            
                // 基本
                { "SalesController.Create", "新增銷售資料" },
                { "SalesController.GetAll", "取得所有銷售資料" },
                { "SalesController.GetById", "依據 ID 取得銷售資料" },
                { "SalesController.Update", "更新銷售資料" },
                { "SalesController.Delete", "刪除銷售資料" },

                { "ProductController.CreateAsync", "新增產品資料" },
                { "ProductController.GetAllAsync", "取得所有產品資料" },
                { "ProductController.GetAsync", "依據 ID 取得產品資料" },
                { "ProductController.UpdateAsync", "更新產品資料" },
                { "ProductController.DeleteAsync", "刪除產品資料" },

                { "OrderController.CreateAsync", "新增訂單資料" },
                { "OrderController.GetAllAsync", "取得所有訂單資料" },
                { "OrderController.GetAsync", "依據 ID 取得訂單資料" },
                { "OrderController.UpdateAsync", "更改訂單資料" },
                { "OrderController.DeleteAsync", "刪除訂單資料" },

                { "DishItemController.CreateAsync", "新增單品資料" },
                { "DishItemController.GetAllAsync", "取得所有單品資料" },
                { "DishItemController.GetAsync", "依據 ID 取得單品資料" },
                { "DishItemController.UpdateAsync", "更新單品資料" },
                { "DishItemController.DeleteAsync", "刪除單品資料" },

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
