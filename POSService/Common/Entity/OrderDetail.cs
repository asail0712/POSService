using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XPlan.Interface;

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

    public class OrderDetail : IEntity
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt { get; set; }                 // 建立時間
        public DateTime UpdatedAt { get; set; }                 // 更新時間
        public List<string> ProductIds { get; set; }            // 產品ID清單
        public decimal TotalPrice { get; set; }                 // 總售價
        public OrderStatus Status { get; set; }                 // 訂單狀態

        public OrderDetail()
        {
            ProductIds  = new List<string>();
            CreatedAt   = DateTime.UtcNow;
            UpdatedAt   = DateTime.UtcNow;
            Status      = OrderStatus.Pending;
        }
    }
}
