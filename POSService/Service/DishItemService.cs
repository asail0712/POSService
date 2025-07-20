using AutoMapper;
using Common.DTO.Dish;
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

        public async Task<DishBriefResponse> GetBriefAsync(string key)
        {
            var item = await _repository.GetAsync(key);

            if (item == null || item.dishStatus == DishStatus.Closed)
            {
                // ED TODO
                return null; // 或者拋出異常，根據你的需求
            }

            return _mapper.Map<DishBriefResponse>(item);
        }

        public async Task<List<DishBriefResponse>> GetAllBriefAsync()
        {
            var items = await _repository.GetAllAsync();

            // 過濾掉 null 和 dishStatus = Closed
            var filteredItems = items
                            .Where(x => x != null && x.dishStatus != DishStatus.Closed)
                            .ToList();
            return _mapper.Map<List<DishBriefResponse>>(items);
        }
    }
}
