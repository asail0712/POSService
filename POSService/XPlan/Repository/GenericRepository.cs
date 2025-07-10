using XPlan.DataAccess;
using XPlan.Interface;

namespace XPlan.Repository
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly IDataAccess<TEntity> _dataAccess;

        public GenericRepository(IDataAccess<TEntity> dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _dataAccess.InsertAsync(entity);
        }

        public Task<IEnumerable<TEntity?>?> GetAllAsync()
        {
            return _dataAccess.QueryAllAsync();
        }

        public Task<TEntity?> GetByIdAsync(string id)
        {
            return _dataAccess.QueryByIdAsync(id);
        }

        public Task<bool> UpdateAsync(string id, TEntity entity)
        {
            return _dataAccess.UpdateAsync(id, entity);
        }

        public Task<bool> DeleteAsync(string id)
        {
            return _dataAccess.DeleteAsync(id);
        }
    }
}
