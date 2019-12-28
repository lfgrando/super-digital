using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Superdigital.Application.Interfaces;
using Superdigital.Domain.Entities;
using Superdigital.Domain.Interfaces.Services;
using Superdigital.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ExecutionContext = Microsoft.Azure.WebJobs.ExecutionContext;

namespace Superdigital.Application.AppServices
{
    public class AppServiceBase<T> : IAppServiceBase<T> where T : EntityBase
    {
        private readonly IServiceBase<T> _serviceBase;
        public AppServiceBase(IServiceBase<T> pServiceBase)
        {
            this._serviceBase = pServiceBase;
        }
        public async Task<Result<long>> DeleteOneAsync(string id, CancellationToken cancellationToken)
        {
            return
                await _serviceBase.DeleteOneAsync(id, cancellationToken);
        }
        public async Task<Result<TResponse>> GetByIdAsync<TResponse>(string id) where TResponse : BaseResponse
        {
            return
                await _serviceBase.GetByIdAsync<TResponse>(id);
        }
        public async Task<Result<ICollection<TResponse>>> GetByFilterAsync<TResponse>(Expression<Func<T, bool>> predicate) where TResponse : BaseResponse
        {
            return
                await _serviceBase.GetByFilterAsync<TResponse>(predicate);
        }
        public async Task<Result<string>> InsertOneAsync<TRequest>(TRequest obj, CancellationToken cancellationToken) where TRequest : BaseRequest
        {
            return
                await _serviceBase.InsertOneAsync(obj, cancellationToken);
        }
        public async Task<Result<string>> ReplaceOneAsync<TRequest>(Expression<Func<T, bool>> filterDefinition, TRequest obj, UpdateOptions options) where TRequest : BaseRequest
        {
            return
                await _serviceBase.ReplaceOneAsync(filterDefinition, obj, options);
        }
        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            _serviceBase.Dispose();
        }        
    }
}