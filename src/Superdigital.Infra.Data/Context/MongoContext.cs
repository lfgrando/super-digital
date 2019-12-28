using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Superdigital.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Superdigital.Infra.Data.Context
{
    public class MongoContext : IMongoContext
    {
        private readonly string _connectionString;
        public MongoContext()
        {
            _connectionString = "mongodb://host.docker.internal:27017/?readPreference=primary";
        }
        

        public string ConnectionString => _connectionString;

        public MongoUrl Url => new MongoUrl(_connectionString);

        public MongoClient Client => new MongoClient(Url);

        public IMongoDatabase Database => Client.GetDatabase("SuperDigital");

        public string GetCollectionName<TEntity>()
        {
            if (Attribute.GetCustomAttribute(typeof(TEntity), typeof(BsonDiscriminatorAttribute)) != null)
            {
                var cm = BsonClassMap.LookupClassMap(typeof(TEntity));
                if (!string.IsNullOrWhiteSpace(cm.Discriminator))
                    return cm.Discriminator;
            }

            var name = typeof(TEntity).Name;
            if (MongoGlobalOptions.EnableCamelCaseCollectionName)
                name = char.ToLower(name[0]) + name.Substring(1);
            return name;
        }

        public IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName) => Database.GetCollection<TEntity>(collectionName);

        public IMongoCollection<TEntity> GetCollection<TEntity>() => GetCollection<TEntity>(GetCollectionName<TEntity>());
    }
}
