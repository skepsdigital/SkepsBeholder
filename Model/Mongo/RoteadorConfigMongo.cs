using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SkepsBeholder.Model.Mongo
{
    public class RoteadorConfigMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Contrato { get; set; }
        public string Roteador { get; set; }
        public List<string> Bots { get; set; }
        public string ChaveRoteador { get; set; }
        public string EmailResponsavel { get; set; }
    }
}
