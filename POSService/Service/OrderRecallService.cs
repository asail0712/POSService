using AutoMapper;
using Common.DTO.Order;
using Common.DTO.OrderRecall;
using Common.DTO.Product;
using MongoDB.Bson;
using Repository.Interface;
using Service.Interface;
using XPlan.Service;

namespace Service
{
    public class OrderRecallService : GenericService<OrderRecallEntity, OrderRecallRequest, OrderRecallResponse, ISalesRepository>, IOrderRecallService
    {
        private readonly IDishItemRepository _dishItemRepository;

        public OrderRecallService(ISalesRepository repo, IMapper mapper, IDishItemRepository dishItemRepository)
            : base(repo, mapper)
        {
            _dishItemRepository = dishItemRepository;
        }

        public async Task<List<OrderRecallResponse>> GetSalesByTime(TimeRangeSalesRequest request)
        {
            List<OrderRecallEntity>? allSold = await _repository.GetByTimeAsync(request.StartTime, request.EndTime);
            return _mapper.Map<List<OrderRecallResponse>>(allSold);
        }

        public async Task<TimeRangeProductSalesResponse> GetProductSalesByTime(TimeRangeProductSalesRequest request)
        {
            var allSold         = await _repository.GetByTimeAsync(request.StartTime, request.EndTime)?? new List<OrderRecallEntity>();

            // 過濾出所有產品並展平
            var allProducts     = allSold.SelectMany(sold => sold.ProductItemList ?? Enumerable.Empty<ProductBrief>());

            // 過濾出目標產品
            var targetProducts  = allProducts.Where(p => p.Id == request.ProductId);

            // 統計次數和總金額
            int purchaseCount   = targetProducts.Count();
            decimal totalAmount = targetProducts.Sum(p => p.Price);

            return new TimeRangeProductSalesResponse
            {
                ProductId       = request.ProductId,
                PurchaseCount   = purchaseCount,
                TotalAmount     = totalAmount
            };
        }

        public async Task<OrderRecallEntity> AddOrderDetail(string orderId, OrderDetailEntity entity)
        {
            // 收集所有需要的 DishItem Id
            var allDishItemIds = entity.ProductPackages
                                .Where(p => p.DishItems != null)
                                .SelectMany(p => p.DishItems!)
                                .Distinct()
                                .ToList();

            // 批次取得所有相關 DishItem
            var allDishItems    = await _dishItemRepository.GetAsync(allDishItemIds.Select(p => p.Id).Distinct().ToList());
            var dishItemMap     = allDishItems?.ToDictionary(d => d.Id, d => d.Name) ?? new Dictionary<string, string>();

            //// 組合 ProductBrief 清單
            var briefList       = entity.ProductPackages.Select(info => new ProductBrief
            {
                Id              = info.Id,
                Name            = info.Name,
                Price           = info.Price,
                MenuNameList    = info.DishItems?
                                    .Where(item => dishItemMap.ContainsKey(item.Id))
                                    .Select(item => dishItemMap[item.Id])
                                    .ToList() ?? new List<string>()
            }).ToList();

            // 建立 SoldItem 並儲存
            var soldItem        = new OrderRecallEntity
            {
                CreatedAt       = entity.CreatedAt,
                UpdatedAt       = entity.UpdatedAt,
                OrderId         = orderId,
                ProductItemList = briefList,
                Price           = entity.TotalPrice
            };

            return await _repository.CreateAsync(soldItem);
        }
    }
}