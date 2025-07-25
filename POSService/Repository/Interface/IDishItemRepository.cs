﻿using Common.DTO.Dish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Repository;

namespace Repository.Interface
{
    public interface IDishItemRepository : IRepository<DishItemEntity>
    {
        Task<int> ReduceStock(string key, int numOfReduce);
    }
}
