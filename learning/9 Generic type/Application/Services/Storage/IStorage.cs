namespace Services.Storage;

public interface IStorage<T>
{
    void Add(T item);
    void Update(T oldItem, T newItem);
    void Delete(T item);
}