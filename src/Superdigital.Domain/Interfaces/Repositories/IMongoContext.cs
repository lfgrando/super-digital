using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Superdigital.Domain.Interfaces.Repositories
{
    public interface IMongoContext
    {
        string ConnectionString { get; }
        MongoUrl Url { get; }
        MongoClient Client { get; }
        IMongoDatabase Database { get; }
        string GetCollectionName<TEntity>();
        IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName);
        IMongoCollection<TEntity> GetCollection<TEntity>();
    }
}
