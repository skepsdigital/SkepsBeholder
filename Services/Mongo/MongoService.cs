using MongoDB.Bson;
using MongoDB.Driver;
using SkepsBeholder.Model.Mongo;

namespace SkepsBeholder.Services.Mongo
{
    public class MongoService : IMongoService
    {
        private readonly IMongoCollection<RoteadorConfigMongo> _roteadorConfigCollection;
        private readonly IMongoCollection<ActionErrorMongo> _actionErrorCollection;

        public MongoService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("Produtos");

            _roteadorConfigCollection = database.GetCollection<RoteadorConfigMongo>("SkepsBeholder_RoteadorConfig");
            _actionErrorCollection = database.GetCollection<ActionErrorMongo>("SkepsBeholder_ActionError");

            // Criar o índice TTL
            var indexKeys = Builders<ActionErrorMongo>.IndexKeys.Ascending(doc => doc.ExpireAt);
            var indexOptions = new CreateIndexOptions { ExpireAfter = TimeSpan.Zero }; // O MongoDB usará a data diretamente
            var indexModel = new CreateIndexModel<ActionErrorMongo>(indexKeys, indexOptions);

            _actionErrorCollection.Indexes.CreateOne(indexModel);
        }

        public async Task<ActionErrorMongo> GetActionErroyByKeyAsync(string key)
        {
            var filter = Builders<ActionErrorMongo>.Filter.Eq(c => c.Key, key);
            return await _actionErrorCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task InsertActionErrorAsync(ActionErrorMongo doc) => await _actionErrorCollection.InsertOneAsync(doc);

        public async Task<RoteadorConfigMongo?> GetRoteadorConfigAsync(string bot)
        {
            return (await _roteadorConfigCollection.Find(new BsonDocument()).ToListAsync())?.FirstOrDefault(r => r.Bots.Contains(bot));
        }

        public async Task AddRoteadorConfigAsync(RoteadorConfigMongo roteadorConfig)
        {
            await _roteadorConfigCollection.InsertOneAsync(roteadorConfig);
        }
    }
}
