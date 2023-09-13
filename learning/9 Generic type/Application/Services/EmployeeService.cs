using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Services.Storage;

namespace Services;

public class EmployeeService
{
    private readonly EmployeeStorage _employeeStorage;

    public EmployeeService(EmployeeStorage employeeStorage)
    {
        _employeeStorage = employeeStorage;
    }

    public void AddEmployee(Employee employee)
    {
        ValidationEmployee(employee);
        _employeeStorage.Add(employee);
    }

    public void UpdateEmployee(Guid employeeId, Employee newEmployee)
    {
        var employee = GetEmployeeById(employeeId);

        ValidationEmployee(newEmployee, true);

        _employeeStorage.Update(employee, newEmployee);
    }

    public void DeleteEmployee(Guid employeeId)
    {
        var employee = GetEmployeeById(employeeId);

        _employeeStorage.Delete(employee);
    }

    public void WithdrawEmployees()
    {
        Console.WriteLine(string.Join('\n',
            _employeeStorage.EmployeeList.Select(employee =>
                $"Firstname: {employee.FirstName}, lastname: {employee.LastName}, contract: {employee.Contract}")));
    }

    private Employee GetEmployeeById(Guid employeeId)
    {
        var bankEmployee =
            _employeeStorage.EmployeeList.SingleOrDefault(employee => employee.EmployeeId.Equals(employeeId));

        if (bankEmployee == null)
            throw new ArgumentException($"The employee with identifier {employeeId} does not exist!",
                nameof(employeeId));

        return bankEmployee;
    }

    private void ValidationEmployee(Employee employee, bool isUpdate = false)
    {
        if (!isUpdate && _employeeStorage.EmployeeList.Contains(employee))
            throw new ArgumentException("This employee has already been added to the banking system!",
                nameof(employee));

        if (string.IsNullOrWhiteSpace(employee.FirstName))
            throw new PropertyValidationException("The employee first name is not specified!",
                nameof(employee.FirstName), nameof(Employee));

        if (string.IsNullOrWhiteSpace(employee.LastName))
            throw new PropertyValidationException("The employee last name is not specified!", nameof(employee.LastName),
                nameof(Employee));

        if (string.IsNullOrWhiteSpace(employee.PhoneNumber))
            throw new PropertyValidationException("The employee phone number is not specified!",
                nameof(employee.PhoneNumber), nameof(Employee));

        if (string.IsNullOrWhiteSpace(employee.Email))
            throw new PropertyValidationException("The employee e-mail is not specified!", nameof(employee.Email),
                nameof(Employee));

        if (string.IsNullOrWhiteSpace(employee.Address))
            throw new PropertyValidationException("The employee address is not specified!", nameof(employee.Address),
                nameof(Employee));

        if (employee.Salary == 0)
            throw new PropertyValidationException("The employee salary is not specified!", nameof(employee.Salary),
                nameof(Employee));

        if (string.IsNullOrWhiteSpace(employee.Contract))
            employee.Contract = $"{employee.FirstName} {employee.LastName}, date of birth: {employee.DateOfBirth}";

        if (employee.DateOfBirth > DateTime.Now || employee.DateOfBirth == DateTime.MinValue ||
            employee.DateOfBirth == DateTime.MaxValue)
            throw new PropertyValidationException("The employee's date of birth is incorrect!",
                nameof(employee.DateOfBirth), nameof(Employee));

        var age = TestDataGenerator.CalculateAge(employee.DateOfBirth);

        if (age < 18)
            throw new PropertyValidationException("Employee is under 18 years old!", nameof(employee.Age),
                nameof(Employee));

        if (age != employee.Age || employee.Age <= 0) employee.Age = age;
    }

    public List<Employee> GetEmployeesByFilters(string firstNameFilter = "", string lastNameFilter = "",
        string phoneNumberFilter = "", string contractFilter = "", decimal? salaryFilter = null,
        DateTime? minDateOfBirth = null, DateTime? maxDateOfBirth = null)
    {
        IEnumerable<Employee> filteredEmployees = _employeeStorage;

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