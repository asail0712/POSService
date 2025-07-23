using AutoMapper;
using Common.DTO.Dish;
using Common.Exceptions;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Service;

namespace Service
{
    public class DishItemService : GenericService<DishItemEntity, DishItemRequest, DishItemResponse, IDishItemRepository>, IDishItemService
    {
        public DishItemService(IDishItemRepository repo, IMapper mapper) 
            : base(repo, mapper)
        {
        }
        // 這裡可以添加特定於 MenuItem 的業務邏輯方法
        // 例如：根據類別獲取餐點、根據價格範圍獲取餐點等

        public async Task<int> ReduceStock(string key, int numOfReduce)
        {
            var dishItem = await _repository.GetAsync(key);
            if (dishItem == null)
            {
                throw new DishItemNotFoundException(key);
            }

            if (dishItem.Stock < numOfReduce)
            {
                throw new DishItemOutOfStockException(key, numOfReduce, dishItem.Stock);
            }

            int reduced = await _repository.ReduceStock(key, numOfReduce);
            if (reduced <= 0)
            {
                throw new InvalidDishItemOperationException("Failed to reduce stock.");
            }

            return reduced;
        }
    }
}
