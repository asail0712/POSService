using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using DataAccess.Interface;
using Repository.Interface;

using XPlan.Repository;
using XPlan.Utility.Caches;
using Common.DTO.TEMPLATE_NAME;

namespace Repository
{
    public class TEMPLATE_NAMERepository : GenericRepository<TEMPLATE_NAMEEntity, ITEMPLATE_NAMEDataAccess>, ITEMPLATE_NAMERepository
    {
        public TEMPLATE_NAMERepository(ITEMPLATE_NAMEDataAccess dataAccess, IMemoryCache memoryCache, IOptions<CacheSettings> cacheSettings) 
            : base(dataAccess, memoryCache, cacheSettings)
        {

        }
    }
}
