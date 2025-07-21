using AutoMapper;
using Common.DTO.Order;
using Common.DTO.Product;
using Repository.Interface;
using Service.Interface;
using System;

using XPlan.Service;

namespace Service
{
    public class OrderService : GenericService<OrderDetailEntity, OrderDetailRequest, OrderDetailResponse, IOrderRepository>, IOrderService
    {
        private readonly IOrderRecallService _saleService;
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepo;

        public OrderService(IOrderRepository repo, IMapper mapper, IOrderRecallService saleService, IProductService productService, IProductRepository productRepo)
            : base(repo, mapper)
        {
            _saleService        = saleService;
            _productService     = productService;
            _productRepo        = productRepo;
        }

        public override async Task<OrderDetailResponse> CreateAsync(OrderDetailRequest request)
        {
            // 檢查產品是否存在
            if (!await _productService.IsExists(request.ProductIds))
            {
                // ED TODO
                throw new Exception($"Product does not exist.");
            }

            var entitys = await _productRepo.GetAsync(request.ProductIds);

            if (entitys.Any(p => p.ProductState == ProductStatus.Closed))
            {
                throw new Exception($"Product not on sale");
            }

            OrderDetailEntity orderDetail   = _mapper.Map<OrderDetailEntity>(request);                  // 創建訂單
            orderDetail.TotalPrice          = await _productService.GetTotalPrice(request.ProductIds);  // 計算總價
            orderDetail.CreatedAt           = DateTime.UtcNow;
            orderDetail.UpdatedAt           = DateTime.UtcNow;
            var entity                      = await _repository.CreateAsync(orderDetail);               // 儲存訂單

            return _mapper.Map<OrderDetailResponse>(entity);                                            // 返回訂單響應
        }

        public override async Task<bool> UpdateAsync(string key, OrderDetailRequest request)
        {
            // 檢查產品是否存在
            if (!await _productService.IsExists(request.ProductIds))
            {
                // ED TODO
                throw new Exception($"Product does not exist.");
            }
            
            OrderDetailEntity orderDetail   = _mapper.Map<OrderDetailEntity>(request);                  // 更新訂單
            orderDetail.UpdatedAt           = DateTime.UtcNow;
            orderDetail.TotalPrice          = await _productService.GetTotalPrice(request.ProductIds);  // 計算總價

            return await _repository.UpdateAsync(key, orderDetail);
        }

        public async Task<bool> ModifyOrderStatus(string orderId, OrderStatus status)
        {
            bool bResult                    = false;
            OrderDetailEntity? orderDetail  = await _repository.GetAsync(orderId);

            if (orderDetail == null)
            {
                throw new Exception($"Order with ID {orderId} does not exist.");
            }

            switch (status)
            {
                case OrderStatus.Completed:
                    // 完成訂單時，更新銷售記錄
                    var entity = _saleService.AddOrderDetail(orderId, orderDetail);
                    if (entity != null)
                    {
                        await _repository.DeleteAsync(orderId);
                    }

                    bResult = entity != null;

                    break;
                case OrderStatus.Cancelled:
                    bResult = await _repository.DeleteAsync(orderId);
                    break;
                default:
                    orderDetail.UpdatedAt   = DateTime.UtcNow;
                    orderDetail.Status      = status;
                    bResult                 = await _repository.UpdateAsync(orderId, orderDetail); ;
                    break;
            }

            return bResult;
        }
    }
}