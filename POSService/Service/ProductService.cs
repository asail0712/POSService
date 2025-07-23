using AutoMapper;

using Common.DTO.Product;
using Common.Exceptions;
using Repository.Interface;

using Service.Interface;
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
                throw new InvalidProductItemException("One or more items in the product do not exist.");
            }

            return await base.CreateAsync(request);
        }

        public override async Task UpdateAsync(string key, ProductPackageRequest request)
        {
            // 確認該產品裡面的Item都是存在的
            foreach (string itemId in request.ItemIDs)
            {
                bool bResult = await _dishItemRepository.ExistsAsync(itemId);
                if (!bResult)
                {
                    throw new ProductNotFoundException(key);
                }
            }

            var entity = _mapper.Map<ProductPackageEntity>(request);

            await _repository.UpdateAsync(key, entity);
        }

        public async Task<ProductBriefResponse> GetBriefAsync(string key)
        {
            var result = await _repository.GetAsync(key);

            if(result.ProductState == ProductStatus.Closed)
            {
                throw new InvalidProductItemException("Product contains unavailable or sold-out items.");
            }
            
            return _mapper.Map<ProductBriefResponse>(result);
        }

        public async Task<List<ProductBriefResponse>> GetAllBriefAsync()
        {            
            var result = await _repository.GetAllAsync();
            
            if (result == null || !result.Any())
            {
                throw new ProductNotFoundException("All Products");
            }

            // 過濾掉包含已售罄或已下架項目的產品
            var validProducts = result.Where(p => p.ProductState != ProductStatus.Closed).ToList();

            return _mapper.Map<List<ProductBriefResponse>>(validProducts);
        }

        public async Task<decimal> GetTotalPrice(List<string> idList)
        {
            var tasks   = idList.Select(id => _repository.GetAsync(id));    // 先準備多個非同步任務
            var results = await Task.WhenAll(tasks);                        // 等待全部完成

            if (results.Any(r => r == null))
            {
                throw new ProductNotFoundException(string.Join(",", idList));
            }

            return results.Sum(item => item.Price);                         // 加總價格
        }
        public async Task ReduceStock(string key, int numOfReduce)
        {
            var product = await _repository.GetAsync(key);

            if (product == null)
            {
                throw new ProductNotFoundException(key);
            }

            foreach (var itemId in product.ItemIDs)
            {
                // 減少每個項目的庫存
                await _dishItemRepository.ReduceStock(itemId, numOfReduce);
            }
        }
    }
}
