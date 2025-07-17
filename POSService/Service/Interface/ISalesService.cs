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
    public interface ISalesService : IService<SoldItemRequest, SoldItemResponse>
    {
        Task<List<SoldItemResponse>> GetSalesByTime(TimeRangeSalesRequest request);
        Task<decimal> GetProductSalesByTime(TimeRangeProductSalesRequest request);
        Task AddOrderDetail(List<string> idList, decimal totalPrice);
    }
}
