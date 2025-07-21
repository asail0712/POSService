using Common.DTO.Order;
using Common.DTO.OrderRecall;
using Common.DTO.Product;
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
    public interface IOrderRecallService : IService<OrderRecallRequest, OrderRecallResponse>
    {
        Task<List<OrderRecallResponse>> GetSalesByTime(TimeRangeSalesRequest request);
        Task<TimeRangeProductSalesResponse> GetProductSalesByTime(TimeRangeProductSalesRequest request);
        Task<OrderRecallEntity> AddOrderDetail(string orderId, OrderDetailEntity entity);
    }
}
