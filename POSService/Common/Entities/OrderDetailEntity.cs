using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using XPlan.Entities;

namespace Common.Entities
{
    public enum OrderStatus
    {
        Pending,    // 待付款
        Paid,       // 已付款
        Shipped,    // 已出貨
        Completed,  // 已完成
        Cancelled   // 已取消
    }

    public class OrderDetailEntity : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }    // 建立時間
        public DateTime UpdatedAt { get; set; }    // 更新時間

        public string OrderId { get; set; }                     // 桌號或訂單編號
        public List<string> ProductIds { get; set; }            // 產品ID清單
        public decimal TotalPrice { get; set; }                 // 總售價
        public OrderStatus Status { get; set; }                 // 訂單狀態
        

        public OrderDetailEntity()
        {
            OrderId     = "";
            ProductIds  = new List<string>();
            Status      = OrderStatus.Pending;
        }
    }
}
