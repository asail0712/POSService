using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class ProductBriefResponse
    {
        public string Name { get; set; }                        // 分類名稱 / 產品名稱
        public bool IsVisible { get; set; }                     // 是否顯示
        public decimal? Cost { get; set; }                      // 價格
        public List<string> Items { get; set; }                 // 菜單項目清單
        public string Descirption { get; set; }                 // 產品描述
    }
}
