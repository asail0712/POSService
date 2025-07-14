using Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class SoldItemRequest
    {
        public Guid MenuItemId { get; set; }
        public List<string>? ProductItemList { get; set; }   // 關聯的餐點 ID
        public DateTime StartTimeAt { get; set; }                   // 開始時間
        public DateTime EndTimeAt { get; set; }                     // 結束時間
    }
}
