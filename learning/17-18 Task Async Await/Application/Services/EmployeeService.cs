using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class EmployeeService
{
    private readonly BankingSystemDbContext _bankingSystemDbContext;

    public EmployeeService(BankingSystemDbContext bankingSystemDbContext)
    {
        _bankingSystemDbContext = bankingSystemDbContext;
    }

    public async Task AddEmployeeAsync(Employee employee)
    {
        await ValidateEmployeeAsync(employee);

        await _bankingSystemDbContext.Employees.AddAsync(employee);

        await _bankingSystemDbContext.SaveChangesAsync();
    }

    public async Task UpdateEmployeeAsync(Guid employeeId, Employee newEmployee)
    {
        var employee = await GetEmployeeByIdAsync(employeeId);

        await ValidateEmployeeAsync(newEmployee);

        employee.FirstName = newEmployee.FirstName;
        employee.LastName = newEmployee.LastName;
        employee.DateOfBirth = newEmployee.DateOfBirth;
        employee.PhoneNumber = newEmployee.PhoneNumber;
        employee.Address = newEmployee.Address;
        employee.Contract = newEmployee.Contract;
        employee.Email = newEmployee.Email;
        employee.Salary = newEmployee.Salary;
        employee.IsOwner = newEmployee.IsOwner;
        employee.Age = newEmployee.Age;
        employee.Bonus = newEmployee.Bonus;

        await _bankingSystemDbContext.SaveChangesAsync();
    }

    public async Task DeleteEmployeeAsync(Guid employeeId)
    {
        var bankEmployee = await GetEmployeeByIdAsync(employeeId);

        await Task.Run(() => _bankingSystemDbContext.Employees.Remove(bankEmployee));

        await _bankingSystemDbContext.SaveChangesAsync();
    }

    private async Task<Employee> GetEmployeeByIdAsync(Guid employeeId)
    {
        var bankEmployee =
            await _bankingSystemDbContext.Employees.SingleOrDefaultAsync(employee =>
                employee.EmployeeId.Equals(employeeId));

        if (bankEmployee == null)
            throw new ValueNotFoundException($"The employee with identifier {employeeId} does not exist!");

        return bankEmployee;
    }

    private async Task ValidateEmployeeAsync(Employee employee, bool isUpdate = false)
    {
        if (!isUpdate &&
            await _bankingSystemDbContext.Employees.AnyAsync(e => e.EmployeeId.Equals(employee.EmployeeId)))
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

        var age = await Task.Run(() => TestDataGenerator.CalculateAge(employee.DateOfBirth));

        if (age < 18)
            throw new PropertyValidationException("Employee is under 18 years old!", nameof(employee.Age),
                nameof(Employee));

        if (age != employee.Age || employee.Age <= 0) employee.Age = age;
    }

    public async Task<IEnumerable<Employee>> EmployeesWithFilterAndPaginationAsync(int page, int pageSize,
        string? firstName = null,
        string? lastName = null, int? age = null, Guid? employeeId = null,
        DateTime? dateOfBirth = null, string? phoneNumber = null, string? address = null, string? email = null,
        string? contract = null, double? salary = null, bool? isOwner = null)
    {
        IQueryable<Employee> query = _bankingSystemDbContext.Employees;

        if (firstName != null)
            query = query.Where(employee => employee.FirstName == firstName);

        if (lastName != null)
            query = query.Where(employee => employee.LastName == lastName);

        if (age != null)
            query = query.Where(employee => employee.Age.Equals((int)age));

        if (dateOfBirth != null)
            query = query.Where(employee => employee.DateOfBirth.Equals(((DateTime)dateOfBirth).ToUniversalTime()));

        if (phoneNumber != null)
            query = query.Where(employee => employee.PhoneNumber == phoneNumber);

        if (address != null)
            query = query.Where(employee => employee.Address == address);

        if (email != null)
            query = query.Where(employee => employee.Email == email);

        if (contract != null)
            query = query.Where(employee => employee.Contract == contract);

        if (salary != null)
            query = query.Where(employee => employee.Salary.Equals(salary));

        if (isOwner != null)
            query = query.Where(employee => employee.IsOwner.Equals(isOwner));

        if (employeeId != null)
            query = query.Where(employee => employee.EmployeeId.Equals(employeeId));

        query = query.OrderBy(employee => employee.FirstName);

        query = query.Skip((page - 1) * pageSize).Take(pageSize);

        return await query.ToListAsync();
    }
}