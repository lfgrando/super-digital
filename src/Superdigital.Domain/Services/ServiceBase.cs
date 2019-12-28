using AutoMapper;
using MongoDB.Driver;
using Superdigital.Domain.AutoMapper;
using Superdigital.Domain.Entities;
using Superdigital.Domain.Interfaces.Repositories;
using Superdigital.Domain.Interfaces.Services;
using Superdigital.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Superdigital.Domain.Services
{
    public class ServiceBase<T> : IDisposable, IServiceBase<T> where T : EntityBase
    {
        private readonly IRepositoryBase<T> _repositoryBase;
        protected readonly IMapper _mapper;
        public ServiceBase(IRepositoryBase<T> pServiceBase)
        {
            var config = AutoMapperConfig.RegisterMappings();
            _mapper = new Mapper(config);
            _repositoryBase = pServiceBase;
        }
        public async Task<Result<TResponse>> GetByIdAsync<TResponse>(string id) where TResponse : BaseResponse
        {
            try
            {
                var obj = await this._repositoryBase.GetByIdAsync(id);
                if (obj == null)
                    return new Result<TResponse>(default(TResponse), HttpStatusCode.NotFound, Failure.GenerateOneFailure($"{typeof(T).Name} not found."));
                TResponse objRequest = this._mapper.Map<T, TResponse>(obj);
                var objResult = new Result<TResponse>(objRequest, HttpStatusCode.OK, null);
                return objResult;
            }
            catch (Exception ex)
            {
                return new Result<TResponse>(default, HttpStatusCode.InternalServerError, Failure.GenerateOneFailure(ex.Message));
            }
        }
        public async Task<Result<ICollection<TResponse>>> GetByFilterAsync<TResponse>(Expression<Func<T, bool>> predicate) where TResponse : BaseResponse
        {
            try
            {
                var obj = await _repositoryBase.GetByFilterAsync(predicate);
                if (obj == null)
                    return new Result<ICollection<TResponse>>(default, HttpStatusCode.NotFound, Failure.GenerateOneFailure($"{typeof(T).Name} not found."));

                ICollection<TResponse> objRequest = this._mapper.Map<ICollection<T>, ICollection<TResponse>>(obj);
                var objResult = new Result<ICollection<TResponse>>(objRequest, HttpStatusCode.OK, null);
                return objResult;
            }
            catch (Exception ex)
            {
                return new Result<ICollection<TResponse>>(default, HttpStatusCode.InternalServerError, Failure.GenerateOneFailure(ex.Message));
            }
        }
        public async Task<Result<string>> InsertOneAsync<TRequest>(TRequest obj, CancellationToken cancellationToken) where TRequest : BaseRequest
        {
            try
            {
                if (obj == null)
                    return
                        new Result<string>(null, HttpStatusCode.BadRequest, Failure.GenerateOneFailure("Object is null."));
                T objModel = this._mapper.Map<TRequest, T>(obj);

                objModel.DataCriacao = DateTime.UtcNow;
                if (!objModel.IsValid(EValidationStage.Insert))
                    return
                        new Result<string>(null, HttpStatusCode.BadRequest, objModel.ValidationErrors);
                if (string.IsNullOrWhiteSpace(objModel.Id))
                    objModel.Id = Guid.NewGuid().ToString();

                objModel.DataCriacao = DateTime.UtcNow;
                string insert = await this._repositoryBase.InsertOneAsync(objModel, cancellationToken);
                if (string.IsNullOrWhiteSpace(insert))
                    return
                        new Result<string>(null, HttpStatusCode.BadRequest, Failure.GenerateOneFailure($"Can't create {typeof(T).Name}."));
                return
                    new Result<string>(insert, HttpStatusCode.Created, null);
            }
            catch (Exception ex)
            {
                return new Result<string>(null, HttpStatusCode.InternalServerError, Failure.GenerateOneFailure(ex.Message));
            }
        }
        public async Task<Result<string>> ReplaceOneAsync<TRequest>(Expression<Func<T, bool>> filterDefinition, TRequest obj, UpdateOptions options) where TRequest : BaseRequest
        {
            try
            {
                if (filterDefinition == null)
                    return
                        new Result<string>(null, HttpStatusCode.BadRequest, Failure.GenerateOneFailure("Filter definition can't be null."));
                if (obj == null)
                    return
                        new Result<string>(null, HttpStatusCode.BadRequest, Failure.GenerateOneFailure("Object is null."));

                T objModel = this._mapper.Map<TRequest, T>(obj);

                if (!objModel.IsValid(EValidationStage.Replace))
                    return
                        new Result<string>(string.Empty, HttpStatusCode.BadRequest, objModel.ValidationErrors);

                string replace = await this._repositoryBase.ReplaceOneAsync(filterDefinition, objModel, options);

                if (string.IsNullOrWhiteSpace(replace))
                    return
                        new Result<string>(string.Empty, HttpStatusCode.BadRequest, Failure.GenerateOneFailure($"{typeof(T).Name} not found."));

                return
                        new Result<string>(replace, HttpStatusCode.OK, null);
            }
            catch (Exception ex)
            {
                return new Result<string>(null, HttpStatusCode.InternalServerError, Failure.GenerateOneFailure(ex.Message));
            }
        }
        public async Task<Result<long>> DeleteOneAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return new Result<long>(0, HttpStatusCode.BadRequest, Failure.GenerateOneFailure("Id can't be null."));

                var delete = await this._repositoryBase.DeleteOneAsync(id, cancellationToken);
                if (delete <= 0)
                    return new Result<long>(delete, HttpStatusCode.NotFound, Failure.GenerateOneFailure($"{typeof(T).Name} not found."));

                return new Result<long>(delete, HttpStatusCode.OK, null);
            }
            catch (Exception ex)
            {
                return new Result<long>(0, HttpStatusCode.InternalServerError, Failure.GenerateOneFailure(ex.Message));
            }
        }
        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            // this._repositoryBase.Dispose();
        }
    }
}