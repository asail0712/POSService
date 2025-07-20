using AutoMapper;
using Common.DTO.Dish;
using Common.DTO.Order;
using Common.DTO.Product;
using DataAccess.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Entities;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.DataAccess;
using XPlan.Utility.Databases;
using static System.Reflection.Metadata.BlobBuilder;

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
            ProductPackageDocument aa =  await doc.ProductDocs[0].ToEntityAsync(); // 確保 ProductDocs 已經被載入

            // 先把 ProductPackageDocument 都取回來
            List<ProductPackageDocument> prodDocList    = (await Task.WhenAll(doc.ProductDocs.Select(prod => prod.ToEntityAsync()))).ToList();
            List<ProductPackageEntity> prodEntList      = new List<ProductPackageEntity>();
            List<DishItemDocument> dishDocList          = new List<DishItemDocument>();

            foreach (var prod in prodDocList)
            {
                ProductPackageEntity ent    = mapper.Map <ProductPackageEntity> (prod);
                dishDocList                 = (await Task.WhenAll(prod.ItemDocs.Select(itemDoc => itemDoc.ToEntityAsync()))).ToList();
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
                ProductDocs     = entity.ProductIds.Select(id => new One<ProductPackageDocument>(id)).ToList()
            };
        }
    }
}