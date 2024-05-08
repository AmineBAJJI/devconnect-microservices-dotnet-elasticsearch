namespace Events.MongoDB;

public interface IRepository<TDocument>
{
    Task<IEnumerable<TDocument>> GetAllAsync();
    Task<TDocument> GetByIdAsync(string id);
    Task CreateAsync(TDocument document);
    Task<bool> UpdateAsync(string id, TDocument document);
    Task<bool> DeleteAsync(string id);
}