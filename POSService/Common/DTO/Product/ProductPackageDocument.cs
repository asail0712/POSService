using Common.DTO.Dish;
using Common.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO.Product
{
    public class ProductPackageDocument : IEntity, XPlan.Entities.IDBEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }                  = "";
        public DateTime CreatedAt { get; set; }                                     // 建立時間
        public DateTime UpdatedAt { get; set; }                                     // 更新時間

        // 實作 IEntity
        public virtual object GenerateNewID() => ObjectId.GenerateNewId().ToString()!;

        /// <inheritdoc />
        public virtual bool HasDefaultID() => string.IsNullOrEmpty(Id);

        // 顯示資訊
        public string Name { get; set; }                = "";                       // 分類名稱 / 產品名稱
        public string ImageUrl { get; set; }            = "";                       // 圖片連結
        public bool IsVisible { get; set; }                                         // 是否顯示
        public bool DisplayWhenSoldOut { get; set; }                                // 菜品缺貨後，是否在前台顯示(顯示售完或是不顯示)
        public decimal? Discount { get; set; }                                      // 可選：群組價格
        public decimal? OverridePrice { get; set; }                                 // 可選：統一設定價格
        public string Description { get; set; }         = "";                       // 產品描述
        public List<One<DishItemDocument>> ItemDocs { get; set; } = new List<One<DishItemDocument>>();   // 菜單項目清單
    }
}
