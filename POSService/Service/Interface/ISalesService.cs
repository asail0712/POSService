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
        Task<int> GetTotalSalesAmount(SoldItemRequest request);
        Task<int> GetConsumptionCount(string id);
        Task AddOrderDetail(List<string> idList, decimal totalPrice);
    }
}
