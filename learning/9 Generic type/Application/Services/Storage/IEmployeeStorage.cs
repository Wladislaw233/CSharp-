using BankingSystemServices.Models;

namespace Services.Storage;

public interface IEmployeeStorage : IStorage<Employee>
{
    List<Employee> EmployeeList { get; }
}