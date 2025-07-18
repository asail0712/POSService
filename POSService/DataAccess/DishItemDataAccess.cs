using AutoMapper;
using Common.Entities;
using Common.Document;
using DataAccess.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.DataAccess;
using XPlan.Utility.Databases;

namespace DataAccess
{
    public class DishItemDataAccess : MongoEntityDataAccess<DishItemEntity, DishItemDocument>, IDishItemDataAccess
    {
        private readonly IMapper _mapper;

        public DishItemDataAccess(IMapper mapper)
            : base()
        {
            _mapper = mapper;
        }

        protected async override Task<DishItemEntity> MapToEntity(DishItemDocument doc)
        {
            return _mapper.Map<DishItemEntity>(doc);
        }

        protected override DishItemDocument MapToDocument(DishItemEntity entity)
        {
            return _mapper.Map<DishItemDocument>(entity);
        }
    }
}
