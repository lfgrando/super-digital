using MongoDB.Driver;
using Superdigital.Domain.Entities;
using Superdigital.Domain.Interfaces.Repositories;
using Superdigital.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Superdigital.Infra.Data.Repositories
{
    public class RepositoryBase<TEntity> : BaseMongoRepository<TEntity> where TEntity : EntityBase
    {
        private readonly dynamic _dataContext;
        public RepositoryBase(IMongoContext context)
        {
            _dataContext = context;
        }
        public override IMongoCollection<TEntity> Collection => _dataContext.GetCollection<TEntity>();
    }
}