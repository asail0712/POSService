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
    public abstract class GenericService<TEntity, TRequest, TResponse>
        : IService<TRequest, TResponse> where TEntity : class, IEntity
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        public GenericService(IRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper     = mapper;
        }

        public async Task CreateAsync(TRequest request)
        {
            var entity = _mapper.Map<TEntity>(request);

            await _repository.CreateAsync(entity);

            return;
        }

        public async Task<IEnumerable<TResponse?>?> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TResponse>>(entities);
        }

        public async Task<TResponse?> GetByIdAsync(string id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TResponse>(entity);
        }

        public async Task<bool> UpdateAsync(string id, TRequest request)
        {
            var entity  = _mapper.Map<TEntity>(request);
            return await _repository.UpdateAsync(id, entity);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
