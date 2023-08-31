using BankingSystemServices.Models;

namespace Services.Storage;

public interface IEmployeeStorage : IStorage<Employee>
{
    void Update(Employee oldEmployee, string? firstName = null, string? lastName = null, int? age = null,
        DateTime? dateOfBirth = null, string? phoneNumber = null, string? address = null, string? email = null,
        string? contract = null, decimal? salary = null, bool? isOwner = null, decimal? bonus = null);

    List<Employee> Data { get; }
}