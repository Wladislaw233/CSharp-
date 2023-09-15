using BankingSystemServices.Models;
using BankingSystemServices.Models.DTO;

namespace Services.Interfaces;

public interface IEmployeeService
{
    public Task<Employee> AddEmployeeAsync(EmployeeDto employeeDto);

    public Task<Employee> UpdateEmployeeAsync(Guid employeeId, EmployeeDto newEmployeeDto);

    public Task DeleteEmployeeAsync(Guid employeeId);

    public Task<Employee> GetEmployeeByIdAsync(Guid employeeId);
}