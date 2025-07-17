using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Entity;
using DataAccess.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

using XPlan.DataAccess;
using XPlan.Utility.Databases;

namespace DataAccess
{
    public class ManagementDataAccess : MongoDataAccess<StaffData>, IManagementDataAccess
    {
        public ManagementDataAccess(IMongoClient database, IOptions<MongoDbSettings> dbSettings)
            : base(database, dbSettings.Value)
        {

        }

        public override async Task<bool> UpdateAsync(string key, StaffData staffData, List<string>? noUpdateList = null)
        {
            return await base.UpdateAsync(key, staffData, new List<string> { "PasswordHash" });
        }

    }
}