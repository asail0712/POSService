using MongoDB.Entities;

using Common.Entities;
using Common.Document;
using DataAccess.Interface;
using Microsoft.Extensions.Options;

using XPlan.DataAccess;
using XPlan.Utility.Databases;

namespace DataAccess
{
    public class ProductDataAccess : MongoEntityDataAccess<ProductPackageEntity, ProductPackageDocument>, IProductDataAccess
    {
        public ProductDataAccess()
            : base()
        {

        }

        protected async override Task<ProductPackageEntity> MapToEntity(ProductPackageDocument doc)
        {
            return new ProductPackageEntity 
            {
                Id                  = doc.Id,
                CreatedAt           = doc.CreatedAt,
                UpdatedAt           = doc.UpdatedAt,
                Name                = doc.Name,
                ImageUrl            = doc.ImageUrl,
                IsVisible           = doc.IsVisible,
                DisplayWhenSoldOut  = doc.DisplayWhenSoldOut,
                Discount            = doc.Discount,
                OverridePrice       = doc.OverridePrice,
                Description         = doc.Description,
                //EntityItems         = (await Task.WhenAll(doc.Items.Select(item => item.ToEntityAsync()))).ToList()
            };
        }

        protected override ProductPackageDocument MapToDocument(ProductPackageEntity entity)
        {
            return new ProductPackageDocument
            {
                Id                  = entity.Id,
                CreatedAt           = entity.CreatedAt,
                UpdatedAt           = entity.UpdatedAt,
                Name                = entity.Name,
                ImageUrl            = entity.ImageUrl,
                IsVisible           = entity.IsVisible,
                DisplayWhenSoldOut  = entity.DisplayWhenSoldOut,
                Discount            = entity.Discount,
                OverridePrice       = entity.OverridePrice,
                Description         = entity.Description,
                //Items               = entity.EntityItems.Select(item => item.ToReference()).ToList()
            };
        }
    }
}
