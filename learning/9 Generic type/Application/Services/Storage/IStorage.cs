namespace Services.Storage;

public interface IStorage<in T>
{
    void Add(T item);

    void Delete(T item);
}