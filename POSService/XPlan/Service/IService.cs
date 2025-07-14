using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.Service
{
    public interface IService<TRequest, TResponse>
    {
        Task<IEnumerable<TResponse?>?> GetAllAsync();
        Task<TResponse?> GetByIdAsync(string id);
        Task CreateAsync(TRequest entity);
        Task<bool> UpdateAsync(string id, TRequest entity);
        Task<bool> DeleteAsync(string id);
    }
}
