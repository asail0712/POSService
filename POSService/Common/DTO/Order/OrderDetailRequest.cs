
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO.Order
{
    public class OrderDetailRequest
    {
        public List<string> ProductIds { get; set; }            // 產品ID清單
    }
}
