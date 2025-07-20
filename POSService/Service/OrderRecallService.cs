using AutoMapper;
using Common.DTO.Order;
using Common.DTO.OrderRecall;
using Common.DTO.Product;
using Repository.Interface;
using Service.Interface;
using XPlan.Service;

namespace Service
{
    public class OrderRecallService : GenericService<OrderRecallEntity, OrderRecallRequest, OrderRecallResponse, ISalesRepository>, IOrderRecallService
    {
        private readonly IProductRepository _productRepository;
        private readonly IDishItemRepository _dishItemRepository;

        public OrderRecallService(ISalesRepository repo, IMapper mapper, IProductRepository productRepository, IDishItemRepository dishItemRepository)
            : base(repo, mapper)
        {
            _productRepository  = productRepository;
            _dishItemRepository = dishItemRepository;
        }

        public async Task<List<OrderRecallResponse>> GetSalesByTime(TimeRangeSalesRequest request)
        {
            List<OrderRecallEntity>? allSold = await _repository.GetByTimeAsync(request.StartTime, request.EndTime);
            return _mapper.Map<List<OrderRecallResponse>>(allSold);
        }

        public async Task<decimal> GetProductSalesByTime(TimeRangeProductSalesRequest request)
        {
            List<OrderRecallEntity> allSold = await _repository.GetByTimeAsync(request.StartTime, request.EndTime)?? new List<OrderRecallEntity>();

            return allSold.SelectMany(soldItem => soldItem.ProductItemList ?? Enumerable.Empty<ProductBrief>())
                .Sum(productItem => productItem.Price);
        }

        public async Task<OrderRecallEntity> AddOrderDetail(string orderId, List<ProductPackageEntity> productList, decimal totalPrice)
        {
            // 收集所有需要的 DishItem Id
            var allDishItemIds = productList
                                .Where(p => p.DishItems != null)
                                .SelectMany(p => p.DishItems!)
                                .Distinct()
                                .ToList();

            // 批次取得所有相關 DishItem
            var allDishItems    = await _dishItemRepository.GetAsync(allDishItemIds.Select(p => p.Id).Distinct().ToList());
            var dishItemMap     = allDishItems?.ToDictionary(d => d.Id, d => d.Name) ?? new Dictionary<string, string>();

            //// 組合 ProductBrief 清單
            var briefList = productList.Select(info => new ProductBrief
            {
                Id              = info.Id,
                Name            = info.Name,
                Price           = info.OverridePrice ?? 0,
                MenuNameList    = info.DishItems?
                                    .Where(item => dishItemMap.ContainsKey(item.Id))
                                    .Select(item => dishItemMap[item.Id])
                                    .ToList() ?? new List<string>()
            }).ToList();

            // 建立 SoldItem 並儲存
            var soldItem = new OrderRecallEntity
            {
                OrderId = orderId,
                ProductItemList = briefList,
                Price = totalPrice
            };

            return await _repository.CreateAsync(soldItem);
        }
    }
}