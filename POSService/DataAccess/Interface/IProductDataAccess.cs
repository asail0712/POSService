using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Entity;

using XPlan.Interface;
    
namespace DataAccess.Interface
{
    public interface IProductDataAccess : IDataAccess<ProductInfo>
    {
    }
}
