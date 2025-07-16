using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.Interface
{
    public interface IEntity
    {
        ObjectId Id { get; set; }           // 唯一識別碼
        DateTime CreatedAt { get; set; }    // 建立時間
        DateTime UpdatedAt { get; set; }    // 更新時間

        // 預設使用Id當作搜尋key
        string SearchKey
        {
            get => Id.ToString();
            set { /* 不做事 */ }
        }
    }
}
