using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Entities;
using DataAccess.Interface;
using Repository.Interface;

using XPlan.Repository;
using XPlan.Utility.Caches;

namespace Repository
{
    public class ProductRepository : GenericRepository<ProductPackageEntity, IProductDataAccess>, IProductRepository
    {
        public ProductRepository(IProductDataAccess dataAccess, IMemoryCache memoryCache, IOptions<CacheSettings> cacheSettings)
            : base(dataAccess, memoryCache, cacheSettings)
        {

        }
    }
}
