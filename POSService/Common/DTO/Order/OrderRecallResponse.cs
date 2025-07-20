using Common.Entities;

namespace Common.DTO.Order
{
    public class OrderRecallResponse
    {
        public string OrderId { get; set; }                         // 訂單編號 
        public List<ProductBrief>? ProductItemList { get; set; }    // 關聯的餐點 ID
        public decimal Amount { get; set; }                         // 銷售金額
    }
}
