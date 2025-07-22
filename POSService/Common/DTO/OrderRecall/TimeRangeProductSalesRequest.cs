
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO.OrderRecall
{
    public class TimeRangeProductSalesRequest : TimeRangeSalesRequest
    {
        public string ProductId { get; set; }   // 產品ID
    }
}
