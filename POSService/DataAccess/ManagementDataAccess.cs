using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Entities;
using DataAccess.Interface;
using Microsoft.Extensions.Options;

using XPlan.DataAccess;
using XPlan.Utility.Databases;

namespace DataAccess
{
    public class ManagementDataAccess : MongoDataAccess<StaffDataEntity>, IManagementDataAccess
    {
        public ManagementDataAccess(IMongoDbContext dbContext, IOptions<MongoDbSettings> dbSettings)
            : base(dbContext, dbSettings.Value)
        {
            EnsureIndexCreated("Account");
        }

        public override async Task<bool> UpdateAsync(string key, StaffDataEntity staffData, List<string>? noUpdateList = null)
        {
            return await base.UpdateAsync(key, staffData, new List<string> { "PasswordHash" });
        }

    }
}