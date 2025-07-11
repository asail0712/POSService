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
        public Guid MenuItemId { get; set; }    // 關聯的餐點 ID
        public DateTime SoldAt { get; set; }    // 銷售時間
        public decimal Amount { get; set; }     // 銷售金額
    }
}
