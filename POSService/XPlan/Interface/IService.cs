using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.Interface
{
    public interface IService<TEntity> where TEntity : IEntity
    {
        Task<IEnumerable<TEntity?>?> GetAllAsync();
        Task<TEntity?> GetByIdAsync(string id);
        Task CreateAsync(TEntity entity);
        Task<bool> UpdateAsync(string id, TEntity entity);
        Task<bool> DeleteAsync(string id);
    }
}
