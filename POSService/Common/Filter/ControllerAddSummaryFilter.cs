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
                { "DishItemController.ReduceStock", "減少菜品庫存" },

                { "ProductController.GetAllBrief", "取得所有產品簡介" },
                { "ProductController.GetBrief", "取得產品簡介" },
                { "ProductController.ReduceStock", "從產品減少菜品庫存" },

                { "OrderController.ModifyOrderStatus", "修改訂單狀態" },

                { "OrderRecallController.GetSalesByTime", "取得銷售總金額" },
                { "OrderRecallController.GetProductSalesByTime", "取得產品消費總金額" },

                { "ManagementController.Login", "登入" },
                { "ManagementController.ChangePassword", "變更密碼" },
                { "ManagementController.ChangeData", "更新後台人員資料" },
            
                // 基本
                { "OrderRecallController.Create", "新增銷售資料" },
                { "OrderRecallController.GetAll", "取得所有銷售資料" },
                { "OrderRecallController.Get", "依據 ID 取得銷售資料" },
                { "OrderRecallController.Update", "更新銷售資料" },
                { "OrderRecallController.Delete", "刪除銷售資料" },

                { "ProductController.Create", "新增產品資料" },
                { "ProductController.GetAll", "取得所有產品資料" },
                { "ProductController.Get", "依據 ID 取得產品資料" },
                { "ProductController.Update", "更新產品資料" },
                { "ProductController.Delete", "刪除產品資料" },

                { "OrderController.Create", "新增訂單資料" },
                { "OrderController.GetAll", "取得所有訂單資料" },
                { "OrderController.Get", "依據 ID 取得訂單資料" },
                { "OrderController.Update", "更改訂單資料" },
                { "OrderController.Delete", "刪除訂單資料" },

                { "DishItemController.Create", "新增單品資料" },
                { "DishItemController.GetAll", "取得所有單品資料" },
                { "DishItemController.Get", "依據 ID 取得單品資料" },
                { "DishItemController.Update", "更新單品資料" },
                { "DishItemController.Delete", "刪除單品資料" },

                { "ManagementController.Create", "新增後台人員資料" },
                { "ManagementController.GetAll", "取得所有後台人員資料" },
                { "ManagementController.Get", "依據 ID 取得後台人員資料" },
                { "ManagementController.Update", "更新後台人員資料" },
                { "ManagementController.Delete", "刪除後台人員資料" },
            };

            return keyValuePairs;
        }
    }
}
