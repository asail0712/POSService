using Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class SoldItemResponse
    {
        public List<string>? ProductItemList { get; set; }          // 關聯的餐點 ID
        public decimal Amount { get; set; }                         // 銷售金額
    }
}
