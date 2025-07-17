using XPlan.Entity;

namespace Common.Entity
{
    public class DishItem : EntityBase
    {
        public string Name { get; set; }          // 餐點名稱
        public string ImageUrl { get; set; }      // 圖片連結
        public decimal Price { get; set; }        // 原價
        public decimal Discount { get; set; }     // 折扣百分比 (0~1)
        public bool IsAvailable { get; set; }     // 販售狀態（true: 上架, false: 下架）
        public int Stock { get; set; }            // 庫存量

        public decimal GetFinalPrice()
        {
            return Math.Round(Price * (1 - Discount), 2);
        }
    }
}
