using System.Collections.Generic;
using System.Threading.Tasks;
using XPlan.Interface;

namespace XPlan.DataAccess
{
    public interface IDataAccess<TEntity> where TEntity : class, IEntity
    {
        Task InsertAsync(TEntity entity);
        Task<IEnumerable<TEntity?>?> QueryAllAsync();
        Task<TEntity?> QueryByIdAsync(string id);
        Task<bool> UpdateAsync(string id, TEntity entity);
        Task<bool> DeleteAsync(string id);
    }
}
