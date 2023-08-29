using BankingSystemServices;

namespace Services.Storage;

public interface IStorage<T>
{
    void Add(T item);
    
    void Delete(T item);
}