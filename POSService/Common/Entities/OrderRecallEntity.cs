using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using XPlan.Entities;

namespace Common.Entities
{
    public class ProductBrief
    {
        public string Id { get; set; }                        // 唯一識別碼 (UUID)
        public string Name { get; set; }                        // 分類名稱 / 產品名稱
        public decimal Price { get; set; }                      // 售價金額
        public List<string> MenuNameList { get; set; }          // 關聯菜品名稱
    }

    public class OrderRecallEntity : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }    // 建立時間
        public DateTime UpdatedAt { get; set; }    // 更新時間

        public string OrderId { get; set; }                     // 桌號或訂單編號
        public List<ProductBrief> ProductItemList { get; set; } // 關聯的餐點 ID
        public decimal Price { get; set; }                      // 銷售金額     
    }
}
