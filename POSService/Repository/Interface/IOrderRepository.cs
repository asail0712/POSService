using Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Interface;

namespace Repository.Interface
{
    public interface IOrderRepository : IRepository<OrderDetail>
    {
    }
}
