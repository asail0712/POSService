using AutoMapper;
using Common.DTO;
using Common.Entity;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Interface;
using XPlan.Service;

namespace Service
{
    public class ProductService : GenericService<ProductInfo, ProductInfoRequest, ProductInfoResponse, IProductRepository>, IProductService
    {
        private IMenuItemRepository _menuItemRepository;

        public ProductService(IProductRepository repo, IMapper mapper, IMenuItemRepository menuItemRepository)
            : base(repo, mapper)
        {
            _menuItemRepository = menuItemRepository;
        }

        public override async Task CreateAsync(ProductInfoRequest request)
        {
            var entity = _mapper.Map<ProductInfo>(request);
            
            // 確認該產品裡面的Item都是存在的
            foreach(string itemId in entity.Items)
            {
                bool bResult = await _menuItemRepository.ExistsAsync(itemId);
                if(!bResult)
                {
                    // ED TODO : Handle the case where the item does not exist
                    return;
                }
            }

            await _repository.CreateAsync(entity);

            return;
        }

        public override async Task<bool> UpdateAsync(string key, ProductInfoRequest request)
        {
            var entity = _mapper.Map<ProductInfo>(request);

            // 確認該產品裡面的Item都是存在的
            foreach (string itemId in entity.Items)
            {
                bool bResult = await _menuItemRepository.ExistsAsync(itemId);
                if (!bResult)
                {
                    // ED TODO : Handle the case where the item does not exist
                    return false;
                }
            }

            return await _repository.UpdateAsync(key, entity);
        }

        public async Task<ProductBriefResponse> GetBriefAsync(string key)
        {
            var result = await _repository.GetAsync(key);

            return _mapper.Map<ProductBriefResponse>(result);
        }
    }
}
