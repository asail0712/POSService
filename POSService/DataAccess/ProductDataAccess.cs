using MongoDB.Entities;

using Common.Entities;
using Common.Document;
using DataAccess.Interface;

using XPlan.DataAccess;
using AutoMapper;

namespace DataAccess
{
    public class ProductDataAccess : MongoEntityDataAccess<ProductPackageEntity, ProductPackageDocument>, IProductDataAccess
    {
        private readonly IMapper _mapper;
        public ProductDataAccess(IMapper mapper)
            : base()
        {
            _mapper = mapper;
        }

        protected async override Task<ProductPackageEntity> MapToEntity(ProductPackageDocument doc)
        {
            List<DishItemDocument> dishList = (await Task.WhenAll(doc.Items.Select(item => item.ToEntityAsync()))).ToList();

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
                DishDocs            = _mapper.Map<List<DishItemEntity>>(dishList)
            };
        }

        protected override ProductPackageDocument MapToDocument(ProductPackageEntity entity)
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
                Items               = entity.Items.Select(id => new One<DishItemDocument>(id)).ToList()
            };
        }
    }
}
