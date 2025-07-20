using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.Entities
{
    public interface IDBEntity
    {
        string Id { get; set; }
        DateTime CreatedAt { get; set; }    // 建立時間
        DateTime UpdatedAt { get; set; }    // 更新時間
    }
}
