using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }                 = false;        // 登入是否成功
        public string ErrorMessage { get; set; }            = string.Empty; // 錯誤訊息
        public StaffDataResponse? StaffData { get; set; }                   // 員工資料
    }
}
