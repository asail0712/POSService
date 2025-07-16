using AutoMapper;
using Common.DTO;
using Common.Entity;
using Repository;
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
        private readonly IDishItemRepository _dishItemRepository;

        public ProductService(IProductRepository repo, IMapper mapper, IDishItemRepository dishItemRepository)
            : base(repo, mapper)
        {
            _dishItemRepository = dishItemRepository;
        }

        public override async Task CreateAsync(ProductInfoRequest request)
        {
            var entity      = _mapper.Map<ProductInfo>(request);            
            // 確認該產品裡面的Item都是存在的
            bool bResult    = await _dishItemRepository.ExistsAsync(entity.Items);

            if(!bResult)
            {
                // ED TODO : Handle the case where the item does not exist
                return;
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
                bool bResult = await _dishItemRepository.ExistsAsync(itemId);
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

        public async Task<decimal> GetTotalPrice(List<string> idList)
        {
            var tasks   = idList.Select(id => _repository.GetAsync(id));    // 先準備多個非同步任務
            var results = await Task.WhenAll(tasks);                        // 等待全部完成
            return results.Sum(item => item.Price);                         // 加總價格
        }
    }
}
