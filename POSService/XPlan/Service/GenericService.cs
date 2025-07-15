using AutoMapper;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XPlan.Interface;
using XPlan.Repository;

namespace XPlan.Service
{
    public abstract class GenericService<TEntity, TRequest, TResponse, TRepository>
        : IService<TRequest, TResponse> where TEntity : class, IEntity where TRepository : IRepository<TEntity>
    {
        protected readonly TRepository _repository;
        protected readonly IMapper _mapper;

        public GenericService(TRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper     = mapper;
        }

        public virtual async Task CreateAsync(TRequest request)
        {
            var entity = _mapper.Map<TEntity>(request);

            await _repository.CreateAsync(entity);

            return;
        }

        public virtual async Task<List<TResponse>?> GetAllAsync()
        {
            List<TEntity>? entities = await _repository.GetAllAsync();
            return _mapper.Map<List<TResponse>?>(entities);
        }

        public virtual async Task<TResponse?> GetAsync(string key)
        {
            var entity = await _repository.GetAsync(key);
            return _mapper.Map<TResponse>(entity);
        }

        public virtual async Task<List<TResponse>?> GetByTimeAsync(DateTime? startTime = null, DateTime? endTime = null)
        {
            List<TEntity>? entities = await _repository.GetByTimeAsync();
            return _mapper.Map<List<TResponse>?>(entities);
        }

        public virtual async Task<bool> UpdateAsync(string key, TRequest request)
        {
            var entity  = _mapper.Map<TEntity>(request);
            return await _repository.UpdateAsync(key, entity);
        }

        public virtual async Task<bool> DeleteAsync(string key)
        {
            return await _repository.DeleteAsync(key);
        }
    }
}
