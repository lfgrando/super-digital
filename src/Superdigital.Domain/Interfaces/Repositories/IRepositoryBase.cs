using MongoDB.Driver;
using Superdigital.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Superdigital.Domain.Interfaces.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        IMongoCollection<TEntity> Collection { get; }
        Task<ICollection<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> predicate);
        Task<string> InsertOneAsync(TEntity entity, CancellationToken cancellationToken);
        Task<TEntity> GetByIdAsync(string id);
        Task<string> ReplaceOneAsync(Expression<Func<TEntity, bool>> filterDefinition, TEntity entity, UpdateOptions options);
        Task<long> DeleteOneAsync(string id, CancellationToken cancellationToken);
    }
}
