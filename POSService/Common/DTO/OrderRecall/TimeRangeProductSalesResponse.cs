
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO.OrderRecall
{
    public class TimeRangeProductSalesResponse
    {
        public string ProductId { get; set; }           // 產品ID
        public int PurchaseCount   { get; set; }        // 消費次數
        public decimal TotalAmount { get; set; }        // 消費總金額
    }
}
