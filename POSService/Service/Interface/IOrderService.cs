using Common.DTO;
using Common.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Service;

namespace Service.Interface
{
    public interface IOrderService : IService<OrderDetailRequest, OrderDetailResponse>
    {
        Task<bool> ModifyOrderStatus(string orderId, OrderStatus status);
    }
}
