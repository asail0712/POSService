using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Entity;
using DataAccess.Interface;
using Repository.Interface;

using XPlan.Interface;
using XPlan.Repository;
using XPlan.Cache;

namespace Repository
{
    public class ManagementRepository : GenericRepository<StaffData>, IManagementRepository
    {
        public ManagementRepository(IManagementDataAccess dataAccess, IMemoryCache memoryCache, IOptions<CacheSettings> cacheSettings)
            : base(dataAccess, memoryCache, cacheSettings)
        {

        }
    }
}
