using MongoDB.Entities;
using DataAccess.Interface;

using XPlan.DataAccess;
using AutoMapper;
using Common.DTO.Dish;
using Common.DTO.Product;

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
            List<DishItemDocument> dishList = (await Task.WhenAll(doc.ItemIDs.Select(item => item.ToEntityAsync()))).ToList();

            if(dishList.Count != doc.ItemIDs.Count)
            {
                throw new InvalidOperationException("Some dish items in the product package do not exist. Please ensure all items are correctly added.");
            }

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
                DishDocs            = mapper.Map<List<DishItemEntity>>(dishList)
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
                ItemIDs             = entity.ItemIDs.Select(id => new One<DishItemDocument>(id)).ToList()
            };
        }
    }
}
