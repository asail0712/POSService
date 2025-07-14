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

        public OrderService(IOrderRepository repo, ISalesRepository saleRepo
            , IMapper mapper)
            : base(repo, mapper)
        {
            _saleRepo = saleRepo;
        }
        // 這裡可以添加特定於 MenuItem 的業務邏輯方法
        // 例如：根據類別獲取餐點、根據價格範圍獲取餐點等

        public async Task<bool> ModifyOrderStatus(string orderId, OrderStatus status)
        {
            bool bResult                = false;
            OrderDetail? orderDetail    = await _repository.GetByIdAsync(orderId);
            
            if (orderDetail == null)
            {
                return false; // 如果找不到訂單，則返回 false
            }

            switch (status)
            {
                case OrderStatus.Completed:
                    // 完成訂單時，更新銷售記錄
                    bResult = await _repository.DeleteAsync(orderId);
                    if(bResult)
                    {
                        SoldItem soldItem = new SoldItem
                        {
                            ProductItemList     = orderDetail.ProductIds,
                            Amount              = orderDetail.TotalPrice,
                            CreatedAt           = DateTime.UtcNow
                        };

                        await _saleRepo.CreateAsync(soldItem);
                    }
                    break;
                case OrderStatus.Cancelled:
                    bResult = await _repository.DeleteAsync(orderId);
                    break;
                default:
                    orderDetail.Status  = status;
                    bResult             = true;
                    break;
            }

            return bResult;
        }
    }
}