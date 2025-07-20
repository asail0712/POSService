using AutoMapper;
using Common.DTO.Order;
using DataAccess.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.DataAccess;
using XPlan.Utility.Databases;

namespace DataAccess
{
    public class OrderDataAccess : MongoEntityDataAccess<OrderDetailEntity, OrderDetailDocument>, IOrderDataAccess
    {
        public OrderDataAccess(IMapper mapper)
            : base(mapper)
        {
            EnsureIndexCreated("OrderId");
        }
    }
}