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
        private ISalesRepository _saleRepo;
        private IProductRepository _productRepository;

        public OrderService(IOrderRepository repo, IMapper mapper, ISalesRepository saleRepo, IProductRepository productRepository)
            : base(repo, mapper)
        {
            _saleRepo           = saleRepo;
            _productRepository  = productRepository;
        }

        public override async Task CreateAsync(OrderDetailRequest request)
        {
            // 檢查產品是否存在
            if (!await _productRepository.ExistsAsync(request.ProductIds))
            {
                // ED TODO
                throw new Exception($"Product does not exist.");
            }
            // 創建訂單
            OrderDetail orderDetail = _mapper.Map<OrderDetail>(request);

            // 計算總價
            orderDetail.TotalPrice = request.ProductIds.Sum(id => _productRepository.GetAsync(id).Result.Price);

            // 儲存訂單
            await _repository.CreateAsync(orderDetail);
        }

        public override async Task<bool> UpdateAsync(string key, OrderDetailRequest request)
        {
            // 檢查產品是否存在
            if (!await _productRepository.ExistsAsync(request.ProductIds))
            {
                // ED TODO
                throw new Exception($"Product does not exist.");
            }
            // 更新訂單
            OrderDetail orderDetail = _mapper.Map<OrderDetail>(request);

            // 計算總價
            orderDetail.TotalPrice = request.ProductIds.Sum(id => _productRepository.GetAsync(id).Result.Price);
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
                        SoldItem soldItem = new SoldItem
                        {
                            ProductItemList = orderDetail.ProductIds,
                            Amount = orderDetail.TotalPrice
                        };

                        await _saleRepo.CreateAsync(soldItem);
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