using MongoDB.Driver;
using Superdigital.Domain.Entities;
using Superdigital.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Superdigital.Application.Interfaces
{
    public interface IAppServiceBase<T>
    {
        Task<Result<long>> DeleteOneAsync(string id, CancellationToken cancellationToken);
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        void Dispose();
        Task<Result<TResponse>> GetByIdAsync<TResponse>(string id) where TResponse : BaseResponse;
        Task<Result<ICollection<TResponse>>> GetByFilterAsync<TResponse>(Expression<Func<T, bool>> predicate) where TResponse : BaseResponse;
        Task<Result<string>> InsertOneAsync<TRequest>(TRequest obj, CancellationToken cancellationToken) where TRequest : Domain.ViewModels.BaseRequest;
        Task<Result<string>> ReplaceOneAsync<TRequest>(Expression<Func<T, bool>> filterDefinition, TRequest obj, UpdateOptions options) where TRequest : Domain.ViewModels.BaseRequest;
    }
}