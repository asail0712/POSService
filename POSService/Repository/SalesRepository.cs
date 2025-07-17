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
    public class SalesRepository : GenericRepository<OrderRecallEntity, ISalesDataAccess>, ISalesRepository
    {
        public SalesRepository(ISalesDataAccess dataAccess, IMemoryCache memoryCache, IOptions<CacheSettings> cacheSettings)
            : base(dataAccess, memoryCache, cacheSettings)
        {

        }
    }
}