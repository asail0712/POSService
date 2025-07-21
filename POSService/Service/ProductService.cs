using AutoMapper;
using Common.DTO.Dish;
using Common.DTO.Product;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MongoDB.Driver;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using XPlan.Service;

namespace Service
{
    public class ProductService : GenericService<ProductPackageEntity, ProductPackageRequest, ProductPackageResponse, IProductRepository>, IProductService
    {
        private readonly IDishItemRepository _dishItemRepository;

        public ProductService(IProductRepository repo, IMapper mapper, IDishItemRepository dishItemRepository)
            : base(repo, mapper)
        {
            _dishItemRepository = dishItemRepository;
        }

        public override async Task<ProductPackageResponse> CreateAsync(ProductPackageRequest request)
        {        
            // 確認該產品裡面的Item都是存在的
            bool bResult    = await _dishItemRepository.ExistsAsync(request.ItemIDs);

            if(!bResult)
            {
                // ED TODO : Handle the case where the item does not exist
                throw new InvalidOperationException("產品中的某些項目不存在。請確認所有項目都已正確添加。");
            }

            return await base.CreateAsync(request);
        }

        public override async Task<bool> UpdateAsync(string key, ProductPackageRequest request)
        {
            // 確認該產品裡面的Item都是存在的
            foreach (string itemId in request.ItemIDs)
            {
                bool bResult = await _dishItemRepository.ExistsAsync(itemId);
                if (!bResult)
                {
                    // ED TODO : Handle the case where the item does not exist
                    return false;
                }
            }

            var entity = _mapper.Map<ProductPackageEntity>(request);

            return await _repository.UpdateAsync(key, entity);
        }

        public async Task<ProductBriefResponse> GetBriefAsync(string key)
        {
            var result = await _repository.GetAsync(key);

            if(result.ProductState == ProductStatus.Closed)
            {
                throw new InvalidOperationException("產品中包含已售罄或不可用的項目，無法顯示簡要資訊。請確認所有項目都可用。");
            }
            
            return _mapper.Map<ProductBriefResponse>(result);
        }

        public async Task<List<ProductBriefResponse>> GetAllBriefAsync()
        {            
            var result = await _repository.GetAllAsync();

            // 過濾掉包含已售罄或已下架項目的產品
            var validProducts = result.Where(p => p.ProductState != ProductStatus.Closed).ToList();

            return _mapper.Map<List<ProductBriefResponse>>(validProducts);
        }

        public async Task<decimal> GetTotalPrice(List<string> idList)
        {
            var tasks   = idList.Select(id => _repository.GetAsync(id));    // 先準備多個非同步任務
            var results = await Task.WhenAll(tasks);                        // 等待全部完成
            return results.Sum(item => item.Price);                         // 加總價格
        }
    }
}
