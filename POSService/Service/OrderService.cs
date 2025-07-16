using AutoMapper;
using Common.DTO;
using Common.Entity;
using MongoDB.Bson;
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
    public class OrderService : GenericService<OrderDetail, OrderDetailRequest, OrderDetailResponse, IOrderRepository>, IOrderService
    {
        private readonly ISalesService _saleService;
        private readonly IProductService _productService;

        public OrderService(IOrderRepository repo, IMapper mapper, ISalesService saleService, IProductService productService)
            : base(repo, mapper)
        {
            _saleService        = saleService;
            _productService     = productService;
        }

        public override async Task CreateAsync(OrderDetailRequest request)
        {
            // 檢查產品是否存在
            if (!await _productService.IsExists(request.ProductIds))
            {
                // ED TODO
                throw new Exception($"Product does not exist.");
            }

            // 創建訂單
            OrderDetail orderDetail = _mapper.Map<OrderDetail>(request);

            // 計算總價
            orderDetail.TotalPrice  = await _productService.GetTotalPrice(request.ProductIds);

            // 儲存訂單
            await _repository.CreateAsync(orderDetail);
        }

        public override async Task<bool> UpdateAsync(string key, OrderDetailRequest request)
        {
            // 檢查產品是否存在
            if (!await _productService.IsExists(request.ProductIds))
            {
                // ED TODO
                throw new Exception($"Product does not exist.");
            }
            // 更新訂單
            OrderDetail orderDetail = _mapper.Map<OrderDetail>(request);

            // 計算總價
            orderDetail.TotalPrice = await _productService.GetTotalPrice(request.ProductIds);

            return await _repository.UpdateAsync(key, orderDetail);
        }

        public async Task<bool> ModifyOrderStatus(string orderId, OrderStatus status)
        {
            bool bResult                = false;
            OrderDetail? orderDetail    = await _repository.GetAsync(orderId);

            if (orderDetail == null)
            {
                return false; // 如果找不到訂單，則返回 false
            }

            switch (status)
            {
                case OrderStatus.Completed:
                    // 完成訂單時，更新銷售記錄
                    bResult = await _repository.DeleteAsync(orderId);
                    if (bResult)
                    {
                        await _saleService.AddOrderDetail(orderDetail.ProductIds, orderDetail.TotalPrice);
                    }
                    break;
                case OrderStatus.Cancelled:
                    bResult = await _repository.DeleteAsync(orderId);
                    break;
                default:
                    orderDetail.Status = status;
                    bResult = true;
                    break;
            }

            return bResult;
        }
    }
}