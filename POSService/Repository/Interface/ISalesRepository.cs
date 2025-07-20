using Common.DTO.OrderRecall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Repository;

namespace Repository.Interface
{
    public interface ISalesRepository : IRepository<OrderRecallEntity>
    {
    }
}
