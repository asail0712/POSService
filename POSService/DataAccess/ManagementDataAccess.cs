﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO.Management;
using DataAccess.Interface;
using Microsoft.Extensions.Options;

using XPlan.DataAccess;
using XPlan.Utility.Databases;

namespace DataAccess
{
    public class ManagementDataAccess : MongoDataAccess<StaffDataEntity>, IManagementDataAccess
    {
        public ManagementDataAccess(IMongoDbContext dbContext)
            : base(dbContext)
        {
            EnsureIndexCreated("Account");
            AddNoUpdateKey("PasswordHash");
        }
    }
}