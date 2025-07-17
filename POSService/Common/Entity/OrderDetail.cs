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
    public enum OrderStatus
    {
        Pending,    // 待付款
        Paid,       // 已付款
        Shipped,    // 已出貨
        Completed,  // 已完成
        Cancelled   // 已取消
    }

    public class OrderDetail : EntityBase
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

        public OrderDetail()
        {
            OrderId     = "";
            ProductIds  = new List<string>();
            Status      = OrderStatus.Pending;
        }
    }
}
