using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO.Dish
{
    public class DishBriefResponse
    {
        public string Id { get; set; }          = "";
        // 顯示資訊
        public string Name { get; set; }        = "";   // 餐點名稱
        public string ImageUrl { get; set; }    = "";   // 圖片連結
        public decimal Price { get; set; }              // 原價格

        // 顯示的狀態
        public DishStatus dishStatus { get; set; }      // 販售狀態
    }
}
