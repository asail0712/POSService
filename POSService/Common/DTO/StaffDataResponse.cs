using Common.Entities;
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
        public string Name { get; set; }         // 名字
        public bool IsActive { get; set; }       // 帳號是否啟用
    }
}
