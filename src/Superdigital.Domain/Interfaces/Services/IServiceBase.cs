using MongoDB.Driver;
using Superdigital.Domain.Entities;
using Superdigital.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Superdigital.Domain.Interfaces.Services
{
    public interface IServiceBase<T> where T : EntityBase
    {
        Task<Result<long>> DeleteOneAsync(string id, CancellationToken cancellationToken);
        Task<Result<TResponse>> GetByIdAsync<TResponse>(string id) where TResponse : BaseResponse;
        Task<Result<ICollection<TResponse>>> GetByFilterAsync<TResponse>(Expression<Func<T, bool>> predicate) where TResponse : BaseResponse;
        Task<Result<string>> InsertOneAsync<TRequest>(TRequest obj, CancellationToken cancellationToken) where TRequest : BaseRequest;
        Task<Result<string>> ReplaceOneAsync<TRequest>(Expression<Func<T, bool>> filterDefinition, TRequest obj, UpdateOptions options) where TRequest : BaseRequest;
        void Dispose();
    }
}