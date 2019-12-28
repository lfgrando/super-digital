using MongoDB.Driver;
using Superdigital.Domain.Entities;
using Superdigital.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Superdigital.Infra.Data.Repositories
{
    public abstract class BaseMongoRepository<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        public abstract IMongoCollection<TEntity> Collection { get; }
        public virtual async Task<ICollection<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(predicate).ToListAsync();
        }
        public async Task<string> InsertOneAsync(TEntity entity, CancellationToken cancellationToken)
        {
            try
            {
                await Collection.InsertOneAsync(entity, new InsertOneOptions { }, cancellationToken);
                return entity.Id;
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new Exception($"{typeof(TEntity).Name} '{entity.Id}' já cadastrado", ex);
            }
        }
        public virtual async Task<long> DeleteOneAsync(string id, CancellationToken cancellationToken)
        {
            var delete = await Collection.DeleteOneAsync(x => x.Id.Equals(id), cancellationToken);
            return delete.DeletedCount;
        }
        public async Task<TEntity> GetByIdAsync(string id)
        {
            return await Collection.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();
        }
        public async Task<string> ReplaceOneAsync(Expression<Func<TEntity, bool>> filterDefinition, TEntity entity, UpdateOptions options)
        {
            var upsert = await Collection.ReplaceOneAsync(filterDefinition, entity, options);
            if (upsert.IsAcknowledged)
                return entity.Id;
            else
                return default;
        }
    }
}