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
using Common.DTO.Management;

namespace Repository
{
    public class ManagementRepository : GenericRepository<StaffDataEntity, IManagementDataAccess>, IManagementRepository
    {
        public ManagementRepository(IManagementDataAccess dataAccess, IMemoryCache memoryCache, IOptions<CacheSettings> cacheSettings)
            : base(dataAccess, memoryCache, cacheSettings)
        {

        }
    }
}
