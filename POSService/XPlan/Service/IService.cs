using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.Service
{
    public interface IService<TRequest, TResponse>
    {
        Task CreateAsync(TRequest request);
        Task<List<TResponse>?> GetAllAsync();
        Task<TResponse?> GetAsync(string key);
        Task<List<TResponse>?> GetAsync(List<string> keys);
        Task<List<TResponse>?> GetByTimeAsync(DateTime? startTime = null, DateTime? endTime = null);        
        Task<bool> UpdateAsync(string key, TRequest request);
        Task<bool> DeleteAsync(string key);
        Task<bool> IsExists(List<string> idList);
    }
}
