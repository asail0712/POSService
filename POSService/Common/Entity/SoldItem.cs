using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XPlan.Interface;

namespace Common.Entity
{
    public class ProductBrief
    {
        public ObjectId Id { get; set; }                        // 唯一識別碼 (UUID)
        public string Name { get; set; }                        // 分類名稱 / 產品名稱
        public decimal Price { get; set; }                      // 售價金額
        public List<string> MenuNameList { get; set; }          // 關聯菜品名稱
    }

    public class SoldItem : IEntity
    {
        public ObjectId Id { get; set; }                        // 唯一識別碼 (UUID)
        public List<ProductBrief> ProductItemList { get; set; } // 關聯的餐點 ID
        public decimal Amount { get; set; }                     // 銷售金額
        public DateTime CreatedAt { get; set; }                 // 建立時間
        public DateTime UpdatedAt { get; set; }                 // 更新時間
    }
}
