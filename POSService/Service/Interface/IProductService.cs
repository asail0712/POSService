using Common.DTO.Dish;
using Common.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Service;

namespace Service.Interface
{
    public interface IProductService : IService<ProductPackageRequest, ProductPackageResponse>
    {
        Task<ProductBriefResponse> GetBriefAsync(string key);
        Task<List<ProductBriefResponse>> GetAllBriefAsync();
        Task<decimal> GetTotalPrice(List<string> idList);
        Task ReduceStock(string key, int numOfReduce);
    }
}
