﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO.Dish;
using XPlan.Service;

namespace Service.Interface
{
    public interface IDishItemService : IService<DishItemRequest, DishItemResponse>
    {
        Task<int> ReduceStock(string key, int numOfReduce);
    }
}
