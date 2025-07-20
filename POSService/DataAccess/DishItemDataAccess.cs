using AutoMapper;
using DataAccess.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.DataAccess;
using XPlan.Utility.Databases;
using Common.DTO.Dish;

namespace DataAccess
{
    public class DishItemDataAccess : MongoEntityDataAccess<DishItemEntity, DishItemDocument>, IDishItemDataAccess
    {
        public DishItemDataAccess(IMapper mapper)
            : base(mapper)
        {
        }
    }
}
