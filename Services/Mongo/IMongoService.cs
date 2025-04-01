using SkepsBeholder.Model.Mongo;

namespace SkepsBeholder.Services.Mongo
{
    public interface IMongoService
    {
        Task AddRoteadorConfigAsync(RoteadorConfigMongo roteadorConfig);
        Task<ActionErrorMongo> GetActionErroyByKeyAsync(string key);
        Task<RoteadorConfigMongo?> GetRoteadorConfigAsync(string roteador);
        Task InsertActionErrorAsync(ActionErrorMongo doc);
    }
}
