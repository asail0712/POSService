using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO.OrderRecall
{
    public class TimeRangeSalesRequest
    {
        public DateTime StartTime   { get; set; }        // 開始時間
        public DateTime EndTime { get; set; }            // 結束時間
    }
}
