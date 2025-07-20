using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;
using Repository.Interface;

using XPlan.Repository;
using XPlan.Utility.Caches;
using Common.DTO.OrderRecall;

namespace Repository
{
    public class OrderRecallRepository : GenericRepository<OrderRecallEntity, ISalesDataAccess>, ISalesRepository
    {
        public OrderRecallRepository(ISalesDataAccess dataAccess, IMemoryCache memoryCache, IOptions<CacheSettings> cacheSettings)
            : base(dataAccess, memoryCache, cacheSettings)
        {

        }
    }
}