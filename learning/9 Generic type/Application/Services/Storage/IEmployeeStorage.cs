using Models;

namespace Services.Storage;

public interface IEmployeeStorage : IStorage<Employee>
{
    List<Employee> Data { get; }
}