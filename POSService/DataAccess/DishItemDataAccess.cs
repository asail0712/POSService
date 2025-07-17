using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Entity;
using DataAccess.Interface;
using Microsoft.Extensions.Options;

using XPlan.DataAccess;
using XPlan.Utility.Databases;

namespace DataAccess
{
    public class DishItemDataAccess : MongoDataAccess<DishItem>, IDishItemDataAccess
    {
        public DishItemDataAccess(IMongoDbContext dbContext, IOptions<MongoDbSettings> dbSettings)
            : base(dbContext, dbSettings.Value)
        {

        }
    }
}
