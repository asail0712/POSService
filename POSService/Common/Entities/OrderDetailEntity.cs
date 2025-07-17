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

    public class OrderDetailEntity : EntityBase
    {
        public string OrderId { get; set; }                     // 桌號或訂單編號
        public List<string> ProductIds { get; set; }            // 產品ID清單
        public decimal TotalPrice { get; set; }                 // 總售價
        public OrderStatus Status { get; set; }                 // 訂單狀態
        
        public override string SearchKey
        {
            get => OrderId.ToString();
            set { /* 不做事 */}
        }

        public OrderDetailEntity()
        {
            OrderId     = "";
            ProductIds  = new List<string>();
            Status      = OrderStatus.Pending;
        }
    }
}
