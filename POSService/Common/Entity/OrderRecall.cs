using XPlan.Entity;

namespace Common.Entity
{
    public class ProductBrief
    {
        public string Id { get; set; }                        // 唯一識別碼 (UUID)
        public string Name { get; set; }                        // 分類名稱 / 產品名稱
        public decimal Price { get; set; }                      // 售價金額
        public List<string> MenuNameList { get; set; }          // 關聯菜品名稱
    }

    public class OrderRecall : EntityBase
    {
        public string OrderId { get; set; }                     // 桌號或訂單編號
        public List<ProductBrief> ProductItemList { get; set; } // 關聯的餐點 ID
        public decimal Price { get; set; }                      // 銷售金額     
    }
}
