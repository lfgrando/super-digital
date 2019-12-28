using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Superdigital.Domain.Entities
{
    public enum EValidationStage
    {
        Insert,
        Replace,
    }
    public abstract class EntityBase
    {
        public EntityBase()
        {
            ValidationErrors = new List<Failure>();
        }
        [BsonId]
        public string Id { get; set; }
        public abstract bool IsValid(EValidationStage eValidationStage);
        [BsonIgnore]
        public IList<Failure> ValidationErrors { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
