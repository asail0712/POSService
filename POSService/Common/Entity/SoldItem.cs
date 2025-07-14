using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XPlan.Interface;

namespace Common.Entity
{
    public class SoldItem : IEntity
    {
        public ObjectId Id { get; set; }                            // 唯一識別碼 (UUID)
        public IEnumerator<string>? ProductItemList { get; set; }   // 關聯的餐點 ID
        public DateTime StartTimeAt { get; set; }                   // 開始時間
        public DateTime EndTimeAt { get; set; }                     // 結束時間
        public decimal Amount { get; set; }                         // 銷售金額
        public DateTime CreatedAt { get; set; }                     // 建立時間
    }
}
