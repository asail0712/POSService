using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.DTO;
using Common.Entity;
using Service.Interface;
using Repository.Interface;

using XPlan.Interface;
using XPlan.Service;

namespace Service
{
    public class MenuItemService : GenericService<MenuItem, MenuItemRequest, MenuItemResponse, IMenuItemRepository>, IMenuItemService
    {
        public MenuItemService(IMenuItemRepository repo, IMapper mapper) 
            : base(repo, mapper)
        {
        }
        // 這裡可以添加特定於 MenuItem 的業務邏輯方法
        // 例如：根據類別獲取餐點、根據價格範圍獲取餐點等

        public async Task<MenuBriefResponse> GetBriefAsync(string key)
        {
            var item = await _repository.GetAsync(key);
            return _mapper.Map<MenuBriefResponse>(item);
        }
    }
}
