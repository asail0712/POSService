﻿using Common.DTO.Dish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.DataAccess;

namespace DataAccess.Interface
{
    public interface IDishItemDataAccess : IDataAccess<DishItemEntity>
    {
        Task<int> ReduceStock(string key, int numOfReduce);
    }
}
