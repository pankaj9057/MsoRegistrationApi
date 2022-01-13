
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DatabaseSettings;
using System.Threading.Tasks;
using System.Collections.Generic;
using MsoRegistrationApi.Interfaces;
namespace MsoRegistrationApi.Services;
public class RegistrationService<T> where T : IBaseInterface
{
    private readonly IMongoCollection<T> _collection;

    // <snippet_ctor>
    public RegistrationService(IOptions<DatabaseSetting> databaseSettings,string collectionName)
    {
        var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseSettings.Value.DatabaseName);

        _collection = mongoDatabase.GetCollection<T>(collectionName);
    }
    // </snippet_ctor>

    public async Task<List<T>> GetAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    public async Task<T> GetAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(T type) =>
        await _collection.InsertOneAsync(type);
  public async Task CreateListAsync(List<T> type) =>
        await _collection.InsertManyAsync(type);
    public async Task UpdateAsync(string id, T type) =>
        await _collection.ReplaceOneAsync(x => x.Id == id, type);

    public async Task RemoveAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);
}