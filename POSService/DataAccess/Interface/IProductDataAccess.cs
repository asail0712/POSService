﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO.Product;
using XPlan.DataAccess;

namespace DataAccess.Interface
{
    public interface IProductDataAccess : IDataAccess<ProductPackageEntity>
    {
    }
}
