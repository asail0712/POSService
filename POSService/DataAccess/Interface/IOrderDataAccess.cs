﻿using Common.DTO.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.DataAccess;

namespace DataAccess.Interface
{
    public interface IOrderDataAccess : IDataAccess<OrderDetailEntity>
    {
    }
}
