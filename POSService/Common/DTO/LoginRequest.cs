using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class LoginRequest
    {
        public string Account { get; set; }     // 帳號
        public string Password { get; set; }    // 密碼（記得存 Hash，不存明文）
    }
}
