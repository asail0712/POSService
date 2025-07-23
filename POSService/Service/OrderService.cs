using AutoMapper;
using Common.DTO.Order;
using Common.DTO.Product;
using Common.Exceptions;
using Repository.Interface;
using Service.Interface;
using System;

using XPlan.Service;
using XPlan.Utility;

namespace Service
{
    public class OrderService : GenericService<OrderDetailEntity, OrderDetailRequest, OrderDetailResponse, IOrderRepository>, IOrderService
    {
        private readonly IOrderRecallService _saleService;
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepo;
        private readonly ISequenceGenerator _sequenceGenerator;

        public OrderService(IOrderRepository repo, IMapper mapper
            , IOrderRecallService saleService
            , IProductService productService
            , IProductRepository productRepo
            , ISequenceGenerator sequenceGenerator)
            : base(repo, mapper)
        {
            _saleService        = saleService;
            _productService     = productService;
            _productRepo        = productRepo;
            _sequenceGenerator  = sequenceGenerator;
        }

        public override async Task<OrderDetailResponse> CreateAsync(OrderDetailRequest request)
        {
            // 檢查產品是否存在
            if (!await _productService.IsExists(request.ProductIds))
            {
                throw new InvalidOrderProductException("One or more products in the order do not exist.");
            }

            var entitys = await _productRepo.GetAsync(request.ProductIds);

            if (entitys.Any(p => p.ProductState == ProductStatus.Closed))
            {
                throw new InvalidOrderProductException("One or more products in the order are not on sale.");
            }

            OrderDetailEntity orderDetail   = _mapper.Map<OrderDetailEntity>(request);                  // 創建訂單
            orderDetail.TotalPrice          = await _productService.GetTotalPrice(request.ProductIds);  // 計算總價
            orderDetail.OrderId             = (await _sequenceGenerator.GetNextSequenceAsync("Order"));   // 生成訂單ID
            orderDetail.CreatedAt           = DateTime.UtcNow;
            orderDetail.UpdatedAt           = DateTime.UtcNow;
            var entity                      = await _repository.CreateAsync(orderDetail);               // 儲存訂單

            return _mapper.Map<OrderDetailResponse>(entity);                                            // 返回訂單響應
        }

        public override async Task UpdateAsync(string key, OrderDetailRequest request)
        {
            // 檢查產品是否存在
            if (!await _productService.IsExists(request.ProductIds))
            {
                throw new InvalidOrderProductException("One or more products in the updated order do not exist.");
            }
            
            OrderDetailEntity orderDetail   = _mapper.Map<OrderDetailEntity>(request);                  // 更新訂單
            orderDetail.UpdatedAt           = DateTime.UtcNow;
            orderDetail.TotalPrice          = await _productService.GetTotalPrice(request.ProductIds);  // 計算總價

            await _repository.UpdateAsync(key, orderDetail);
        }

        public async Task ModifyOrderStatus(string orderId, OrderStatus status)
        {
            OrderDetailEntity? orderDetail  = await _repository.GetAsync(orderId);

            if (orderDetail == null)
            {
                throw new OrderNotFoundException(orderId);
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
                    else
                    {
                        throw new OrderStatusUpdateException(orderId, status);
                    }

                    break;
                case OrderStatus.Cancelled:
                    await _repository.DeleteAsync(orderId);
                    break;
                default:
                    orderDetail.UpdatedAt   = DateTime.UtcNow;
                    orderDetail.Status      = status;
                    await _repository.UpdateAsync(orderId, orderDetail); ;
                    break;
            }
        }
    }
}