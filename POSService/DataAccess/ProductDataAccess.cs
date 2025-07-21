using AutoMapper;
using Common.DTO.Dish;
using Common.DTO.Product;
using DataAccess.Interface;
using MongoDB.Driver;
using MongoDB.Entities;
using XPlan.DataAccess;

namespace DataAccess
{
    public class ProductDataAccess : MongoEntityDataAccess<ProductPackageEntity, ProductPackageDocument>, IProductDataAccess
    {
        public ProductDataAccess(IMapper mapper)
            : base(mapper)
        {
        }

        protected async override Task<ProductPackageEntity> MapToEntity(ProductPackageDocument doc, IMapper mapper)
        {
            List<DishItemDocument> dishList = (await Task.WhenAll(doc.ItemDocs.Select(item => item.ToEntityAsync()))).ToList();

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
                DishItems           = mapper.Map<List<DishItemEntity>>(dishList)
            };
        }

        protected override ProductPackageDocument MapToDocument(ProductPackageEntity entity, IMapper mapper)
        {
            return  new ProductPackageDocument
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
                ItemDocs            = entity.ItemIDs.Select(id => new One<DishItemDocument>(id)).ToList()
            };
        }
    }
}
