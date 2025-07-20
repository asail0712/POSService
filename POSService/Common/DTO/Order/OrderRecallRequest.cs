using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO.Order
{
    public class OrderRecallRequest
    {
        public string OrderId { get; set; }                     // 桌號或訂單編號
        public List<string> ProductIds { get; set; }            // 產品ID清單
        public decimal TotalPrice { get; set; }                 // 總售價
    }
}
