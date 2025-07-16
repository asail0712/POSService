using Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class TimeRangeDishSalesRequest : TimeRangeSalesRequest
    {
        public string DishId { get; set; }   // 菜品ID
    }
}
