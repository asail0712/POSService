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
    public class ProductDataAccess : MongoDataAccess<ProductPackageEntity>, IProductDataAccess
    {
        public ProductDataAccess(IMongoDbContext dbContext, IOptions<MongoDbSettings> dbSettings)
            : base(dbContext, dbSettings.Value)
        {

        }
    }
}
