using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Superdigital.Infra.Data.Context
{
    public static class MongoGlobalOptions
    {
        public static void Init()
        {
            EnableCamelCaseCollectionName = true;
            EnableCamelCaseElementName = true;
            EnableEnumRepresentationString = true;
        }

        #region Private members
        private static bool _enableCamelCaseElementName;
        private static bool _enableLocalDateTimeSerialization;
        private static bool _enableEnumRepresentationString;
        #endregion

        #region Public members
        public static bool EnableCamelCaseElementName
        {
            get { return _enableCamelCaseElementName; }
            set
            {
                if (_enableCamelCaseElementName == value)
                    return;
                //
                _enableCamelCaseElementName = value;
                // Add new conventions
                var conventions = new ConventionPack();
                if (_enableCamelCaseElementName)
                    conventions.Add(new CamelCaseElementNameConvention());
                //
                ConventionRegistry.Register("Camel case element name convention", conventions, t => true);
            }
        }

        public static bool EnableCamelCaseCollectionName { get; set; }

        public static bool EnableLocalDateTimeSerialization
        {
            get { return _enableLocalDateTimeSerialization; }
            set
            {
                if (_enableLocalDateTimeSerialization == value)
                    return;

                _enableLocalDateTimeSerialization = value;
                if (_enableLocalDateTimeSerialization)
                {
                    var serializer = new DateTimeSerializer(DateTimeKind.Local);
                    BsonSerializer.RegisterSerializer(typeof(DateTime), serializer);
                }
            }
        }

        public static bool EnableEnumRepresentationString
        {
            get { return _enableEnumRepresentationString; }
            set
            {
                if (_enableEnumRepresentationString == value)
                    return;
                //
                _enableEnumRepresentationString = value;
                // Add new conventions
                var conventions = new ConventionPack();
                if (_enableEnumRepresentationString)
                    conventions.Add(new EnumRepresentationConvention(BsonType.String));
                //
                ConventionRegistry.Register("Enum representation convention string", conventions, t => true);
            }
        }
        #endregion
    }
}
