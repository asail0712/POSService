using AutoMapper;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XPlan.Entities;
using XPlan.Repository;

namespace XPlan.Service
{
    public abstract class GenericService<TEntity, TRequest, TResponse, TRepository>
        : IService<TRequest, TResponse> where TEntity : IDBEntity where TRepository : IRepository<TEntity>
    {
        protected readonly TRepository _repository;
        protected readonly IMapper _mapper;

        public GenericService(TRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper     = mapper;
        }

        public virtual async Task<TResponse> CreateAsync(TRequest request)
        {
            var entity          = _mapper.Map<TEntity>(request);
            entity.CreatedAt    = DateTime.UtcNow;
            entity.UpdatedAt    = DateTime.UtcNow;
            entity              = await _repository.CreateAsync(entity);

            return _mapper.Map<TResponse>(entity);
        }

        public virtual async Task<List<TResponse>> GetAllAsync()
        {
            List<TEntity>? entities = await _repository.GetAllAsync();
            return _mapper.Map<List<TResponse>>(entities);
        }

        public virtual async Task<TResponse> GetAsync(string key)
        {
            var entity = await _repository.GetAsync(key);
            return _mapper.Map<TResponse>(entity);
        }
        public virtual async Task<List<TResponse>> GetAsync(List<string> keys)
        {
            var entity = await _repository.GetAsync(keys);
            return _mapper.Map<List<TResponse>>(entity);
        }

        public virtual async Task<List<TResponse>> GetByTimeAsync(DateTime? startTime = null, DateTime? endTime = null)
        {
            List<TEntity>? entities = await _repository.GetByTimeAsync();
            return _mapper.Map<List<TResponse>?>(entities);
        }

        public virtual async Task<bool> UpdateAsync(string key, TRequest request)
        {
            var entity          = _mapper.Map<TEntity>(request);
            entity.UpdatedAt    = DateTime.UtcNow;

            return await _repository.UpdateAsync(key, entity);
        }

        public virtual async Task<bool> DeleteAsync(string key)
        {
            return await _repository.DeleteAsync(key);
        }

        public async Task<bool> IsExists(List<string> idList)
        {
            return await _repository.ExistsAsync(idList);
        }
    }
}
