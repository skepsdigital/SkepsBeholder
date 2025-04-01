using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SkepsBeholder.Model.Mongo
{
    public class ActionErrorMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Key { get; set; }

        [BsonElement("ExpireAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime ExpireAt { get; set; }
    }
}
