using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace Events.MongoDB;

public class Repository<TDocument> : IRepository<TDocument>
{
    private readonly IMongoCollection<TDocument> _collection;

    public Repository(IMongoClient mongoClient, IOptions<MongoDBSettings> settings)
    {
        var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _collection = database.GetCollection<TDocument>(settings.Value.CollectionName);
    }

    public async Task<IEnumerable<TDocument>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<TDocument> GetByIdAsync(string id)
    {
        var objectId = new ObjectId(id);
        var filter = Builders<TDocument>.Filter.Eq("_id", objectId);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(TDocument document)
    {
        await _collection.InsertOneAsync(document);
    }

    public async Task<bool> UpdateAsync(string id, TDocument document)
    {
        var objectId = new ObjectId(id);
        var filter = Builders<TDocument>.Filter.Eq("_id", objectId);
        var updateResult = await _collection.ReplaceOneAsync(filter, document);
        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var objectId = new ObjectId(id);
        var filter = Builders<TDocument>.Filter.Eq("_id", objectId);
        var deleteResult = await _collection.DeleteOneAsync(filter);
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
}