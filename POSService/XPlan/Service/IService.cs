using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Interface;

namespace XPlan.Service
{
    public interface IService<TRequest, TResponse>
    {
        Task CreateAsync(TRequest request);
        Task<List<TResponse>?> GetAllAsync();
        Task<TResponse?> GetAsync(string key);
        Task<List<TResponse>?> GetByTimeAsync(DateTime? startTime = null, DateTime? endTime = null);        
        Task<bool> UpdateAsync(string key, TRequest request);
        Task<bool> DeleteAsync(string key);
    }
}
