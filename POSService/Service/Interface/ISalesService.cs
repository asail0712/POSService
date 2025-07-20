using Common.DTO;
using Common.DTO.Order;
using Common.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Service;

namespace Service.Interface
{
    public interface ISalesService : IService<OrderRecallRequest, OrderRecallResponse>
    {
        Task<List<OrderRecallResponse>> GetSalesByTime(TimeRangeSalesRequest request);
        Task<decimal> GetProductSalesByTime(TimeRangeProductSalesRequest request);
        Task AddOrderDetail(string orderId, List<string> idList, decimal totalPrice);
    }
}
