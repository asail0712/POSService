using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO.Dish
{
    public class DishItemRequest
    {
        // 顯示資訊
        public string Name { get; set; }        = "";   // 餐點名稱
        public string ImageUrl { get; set; }    = "";   // 圖片連結
        public decimal Price { get; set; }              // 原價

        // 顯示的狀態
        public bool IsAvailable { get; set; }           // 販售狀態（true: 上架, false: 下架）
        public int Stock { get; set; }                  // 庫存量
    }
}
