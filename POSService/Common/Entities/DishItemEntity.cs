using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using XPlan.Entities;

namespace Common.Entities
{
    public class DishItemEntity : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }    // 建立時間
        public DateTime UpdatedAt { get; set; }    // 更新時間

        public string Name { get; set; }          // 餐點名稱
        public string ImageUrl { get; set; }      // 圖片連結
        public decimal Price { get; set; }        // 原價
        public decimal Discount { get; set; }     // 折扣百分比 (0~1)
        public bool IsAvailable { get; set; }     // 販售狀態（true: 上架, false: 下架）
        public int Stock { get; set; }            // 庫存量

        public decimal GetFinalPrice()
        {
            return Math.Round(Price * (1 - Discount), 2);
        }
    }
}
