using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.DTO;
using XPlan.Service;

namespace Service.Interface
{
    public interface IProductService : IService<ProductPackageRequest, ProductPackageResponse>
    {
        Task<ProductBriefResponse> GetBriefAsync(string key);
        Task<decimal> GetTotalPrice(List<string> idList);
    }
}
