using AutoMapper;
using Common.DTO.Product;
using Common.Entities;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
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
            bool bResult    = await _dishItemRepository.ExistsAsync(request.Items);

            if(!bResult)
            {
                // ED TODO : Handle the case where the item does not exist
                throw new InvalidOperationException("產品中的某些項目不存在。請確認所有項目都已正確添加。");
            }

            var entity      = _mapper.Map<ProductPackageEntity>(request);
            entity          = await _repository.CreateAsync(entity);

            return _mapper.Map<ProductPackageResponse>(entity);
        }

        public override async Task<bool> UpdateAsync(string key, ProductPackageRequest request)
        {
            // 確認該產品裡面的Item都是存在的
            foreach (string itemId in request.Items)
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

            return _mapper.Map<ProductBriefResponse>(result);
        }

        public async Task<List<ProductBriefResponse>> GetAllBriefAsync()
        {
            var result = await _repository.GetAllAsync();

            return _mapper.Map<List<ProductBriefResponse>>(result);
        }

        public async Task<decimal> GetTotalPrice(List<string> idList)
        {
            var tasks   = idList.Select(id => _repository.GetAsync(id));    // 先準備多個非同步任務
            var results = await Task.WhenAll(tasks);                        // 等待全部完成
            return results.Sum(item => item.Price);                         // 加總價格
        }
    }
}
