using AutoMapper;
using Common.DTO.Dish;
using Common.DTO.Order;
using Common.DTO.Product;
using DataAccess.Interface;

using XPlan.DataAccess;

namespace DataAccess
{
    public class OrderDataAccess : MongoEntityDataAccess<OrderDetailEntity, OrderDetailDocument>, IOrderDataAccess
    {
        public OrderDataAccess(IMapper mapper)
            : base(mapper)
        {
            EnsureIndexCreated("OrderId");
        }

        protected async override Task<OrderDetailEntity> MapToEntity(OrderDetailDocument doc, IMapper mapper)
        {   
            // 先把 ProductPackageDocument 都取回來
            List<ProductPackageDocument> prodDocList    = (await Task.WhenAll(doc.ProductDocs.Select(prod => prod.LoadEntityAsync()))).ToList();
            List<ProductPackageEntity> prodEntList      = new List<ProductPackageEntity>();
            List<DishItemDocument> dishDocList          = new List<DishItemDocument>();

            foreach (var prod in prodDocList)
            {
                ProductPackageEntity ent    = mapper.Map <ProductPackageEntity> (prod);
                dishDocList                 = (await Task.WhenAll(prod.ItemDocs.Select(itemDoc => itemDoc.LoadEntityAsync()))).ToList();
                ent.DishItems               = mapper.Map<List<DishItemEntity>>(dishDocList);

                prodEntList.Add(ent);
            }

            return new OrderDetailEntity
            {
                Id              = doc.Id,
                CreatedAt       = doc.CreatedAt,
                UpdatedAt       = doc.UpdatedAt,
                OrderId         = doc.OrderId,
                TotalPrice      = doc.TotalPrice,
                Status          = doc.Status,
                ProductPackages = prodEntList
            };
        }

        protected override OrderDetailDocument MapToDocument(OrderDetailEntity entity, IMapper mapper)
        {
            return new OrderDetailDocument
            {
                Id              = entity.Id,
                CreatedAt       = entity.CreatedAt,
                UpdatedAt       = entity.UpdatedAt,
                OrderId         = entity.OrderId,
                TotalPrice      = entity.TotalPrice,
                Status          = entity.Status,
                ProductDocs     = entity.ProductIds.Select(id => id.ToOne<ProductPackageDocument>()).ToList()
            };
        }
    }
}