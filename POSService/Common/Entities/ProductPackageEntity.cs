﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using XPlan.Entities;

namespace Common.Entities
{
    public class ProductPackageEntity : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }                  = "";
        public DateTime CreatedAt { get; set; }                                     // 建立時間
        public DateTime UpdatedAt { get; set; }                                     // 更新時間

        // 顯示資訊
        public string Name { get; set; }                = "";                       // 分類名稱 / 產品名稱
        public string ImageUrl { get; set; }            = "";                       // 圖片連結
        public bool IsVisible { get; set; }                                         // 是否顯示
        public bool DisplayWhenSoldOut { get; set; }                                // 菜品缺貨後，是否在前台顯示(顯示售完或是不顯示)
        public decimal? Discount { get; set; }                                      // 可選：群組價格
        public decimal? OverridePrice { get; set; }                                 // 可選：統一設定價格
        public string Descirption { get; set; }         = "";                       // 產品描述
        public List<DishItemEntity> Items { get; set; } = new List<DishItemEntity>();   // 菜單項目清單

        public decimal Price
        {
            get
            {
                if (OverridePrice.HasValue)
                {
                    if (Discount.HasValue)
                    {
                        return OverridePrice.Value - Discount.Value;
                    }
                    else
                    {
                        return OverridePrice.Value;
                    }                    
                }
                else
                {
                    decimal totalPrice = 0;

                    foreach(var item in Items)
                    {
                        totalPrice += item.Price;
                    }

                    if (Discount.HasValue)
                    {
                        totalPrice -= Discount.Value;
                    }

                    totalPrice = totalPrice <  0 ? 0 : totalPrice; // 確保價格不為負數

                    return totalPrice;
                }
            }
        }
    }
}
