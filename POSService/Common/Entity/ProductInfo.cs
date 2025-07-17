using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XPlan.Entity;

namespace Common.Entity
{
    public class ProductInfo : EntityBase
    {
        public string Name { get; set; }                        // 分類名稱 / 產品名稱
        public bool IsVisible { get; set; }                     // 是否顯示
        public decimal? Discount { get; set; }                  // 可選：群組折扣 (0~1)
        public decimal? OverridePrice { get; set; }             // 可選：統一設定價格
        public List<string> Items { get; set; }                 // 菜單項目清單

        public decimal Price
        {
            get
            {
                if (OverridePrice.HasValue && Discount.HasValue)
                {
                    return OverridePrice.Value * Discount.Value;
                }
                else if (OverridePrice.HasValue)
                {
                    return OverridePrice.Value; 
                }

                return 0;
            }
        }
    }
}
