using Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class StaffDataResponse
    {
        public string Account { get; set; }      // 帳號
        public string PasswordHash { get; set; } // 密碼（記得存 Hash，不存明文）
        public string Name { get; set; }         // 名字
        public bool IsActive { get; set; }       // 帳號是否啟用
        public DateTime CreatedAt { get; set; }  // 建立時間
        public DateTime UpdatedAt { get; set; }  // 更新時間
    }
}
