using Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class OrderDetailResponse
    {
        public string OrderId { get; set; }                     // 桌號或訂單編號
        public DateTime CreatedAt { get; set; }                 // 建立時間
        public DateTime UpdatedAt { get; set; }                 // 更新時間
        public List<string> ProductIds { get; set; }            // 產品ID清單
        public decimal TotalPrice { get; set; }                 // 總售價
        public OrderStatus Status { get; set; }                 // 訂單狀態
    }
}
