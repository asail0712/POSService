using Common.DTO.Dish;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO.Product
{
    public class ProductBriefResponse
    {
        public string Id { get; set; }                  = "";
        public string Name { get; set; }                = "";                               // 分類名稱 / 產品名稱
        public string ImageUrl { get; set; }            = "";                               // 圖片連結
        public decimal? Price { get; set; }                                                 // 設定價格
        public List<DishBriefResponse> DishDocs { get; set; } = new List<DishBriefResponse>();    // 菜單項目清單
        public string Descirption { get; set; }         = "";                               // 產品描述
    }
}
