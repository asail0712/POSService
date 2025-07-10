using System.Collections.Generic;
using System.Threading.Tasks;

namespace XPlan.DataAccess
{
    public interface IDataAccess<TEntity> where TEntity : class
    {
        Task<TEntity?> InsertAsync(TEntity entity);
        Task<IEnumerable<TEntity?>?> QueryAllAsync();
        Task<TEntity?> QueryByIdAsync(string id);
        Task<bool> UpdateAsync(string id, TEntity entity);
        Task<bool> DeleteAsync(string id);
    }
}
