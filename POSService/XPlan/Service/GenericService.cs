using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XPlan.Interface;

namespace XPlan.Service
{
    public abstract class GenericService<TEntity>
        : IService<TEntity> where TEntity : class, IEntity
    {
        private readonly IRepository<TEntity> _repository;

        public GenericService(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<TEntity?> CreateAsync(TEntity entity)
        {
            await _repository.CreateAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<TEntity?>?> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<TEntity?> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(string id, TEntity entity)
        {
            return await _repository.UpdateAsync(id, entity);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
