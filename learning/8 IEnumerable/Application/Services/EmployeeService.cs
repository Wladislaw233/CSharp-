using BankingSystemServices.Models;
using BankingSystemServices.Services;
using BankingSystemServices.Exceptions;
using Services.Storage;

namespace Services;

public class EmployeeService
{
    public static List<Employee> GetEmployeesByFilters(EmployeeStorage employeeStorage,
        string firstNameFilter = "",
        string lastNameFilter = "", string phoneNumberFilter = "", string contractFilter = "",
        decimal? salaryFilter = null, DateTime? minDateOfBirth = null,
        DateTime? maxDateOfBirth = null)
    {
        IEnumerable<Employee> filteredEmployees = employeeStorage;
        if (!string.IsNullOrWhiteSpace(firstNameFilter))
            filteredEmployees = filteredEmployees.Where(employee => employee.FirstName == firstNameFilter);
        if (!string.IsNullOrWhiteSpace(lastNameFilter))
            filteredEmployees = filteredEmployees.Where(employee => employee.LastName == lastNameFilter);
        if (!string.IsNullOrWhiteSpace(phoneNumberFilter))
            filteredEmployees = filteredEmployees.Where(employee => employee.PhoneNumber == phoneNumberFilter);
        if (!string.IsNullOrWhiteSpace(contractFilter))
            filteredEmployees = filteredEmployees.Where(employee => employee.Contract == contractFilter);
        if (salaryFilter.HasValue)
            filteredEmployees = filteredEmployees.Where(employee => employee.Salary == salaryFilter);
        if (minDateOfBirth.HasValue)
            filteredEmployees = filteredEmployees.Where(employee => employee.DateOfBirth >= minDateOfBirth);
        if (maxDateOfBirth.HasValue)
            filteredEmployees = filteredEmployees.Where(employee => employee.DateOfBirth <= maxDateOfBirth);
        
        return filteredEmployees.ToList();
    }
}