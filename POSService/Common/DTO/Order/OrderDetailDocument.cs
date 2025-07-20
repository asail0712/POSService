using Common.DTO.Product;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;
using XPlan.Entities;

namespace Common.DTO.Order
{
    public class OrderDetailDocument : IEntity, IDBEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }                 // 建立時間
        public DateTime UpdatedAt { get; set; }                 // 更新時間
       
        // 實作 IEntity
        public object GenerateNewID()   => ObjectId.GenerateNewId().ToString()!;
        public bool HasDefaultID()      => string.IsNullOrEmpty(Id);

        public string OrderId { get; set; }                     // 桌號或訂單編號
        public List<One<ProductPackageDocument>> ProductDocs { get; set; }   // 產品ID清單
        public decimal TotalPrice { get; set; }                 // 總售價
        public OrderStatus Status { get; set; }                 // 訂單狀態
        

        public OrderDetailDocument()
        {
            OrderId     = "";
            Status      = OrderStatus.Pending;
        }
    }
}
