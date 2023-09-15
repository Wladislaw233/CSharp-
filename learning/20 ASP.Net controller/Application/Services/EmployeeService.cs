using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Models.DTO;
using BankingSystemServices.Services;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services;

public class EmployeeService : IEmployeeService
{
    private readonly BankingSystemDbContext _bankingSystemDbContext;

    public EmployeeService(BankingSystemDbContext bankingSystemDbContext)
    {
        _bankingSystemDbContext = bankingSystemDbContext;
    }

    public async Task<Employee> AddEmployeeAsync(EmployeeDto employeeDto)
    {
        var employee = MapDtoToEmployee(employeeDto);

        await ValidateEmployeeAsync(employee);

        await _bankingSystemDbContext.Employees.AddAsync(employee);

        await _bankingSystemDbContext.SaveChangesAsync();

        return employee;
    }

    public async Task<Employee> UpdateEmployeeAsync(Guid employeeId, EmployeeDto newEmployeeDto)
    {
        var employee = await GetEmployeeByIdAsync(employeeId);

        employee = MapDtoToEmployee(newEmployeeDto, employee);

        await ValidateEmployeeAsync(employee, true);

        _bankingSystemDbContext.Employees.Update(employee);

        await _bankingSystemDbContext.SaveChangesAsync();

        return employee;
    }

    public async Task DeleteEmployeeAsync(Guid employeeId)
    {
        var bankEmployee = await GetEmployeeByIdAsync(employeeId);

        _bankingSystemDbContext.Employees.Remove(bankEmployee);

        await _bankingSystemDbContext.SaveChangesAsync();
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

    public async Task<Employee> GetEmployeeByIdAsync(Guid employeeId)
    {
        var employee = await _bankingSystemDbContext.Employees.SingleOrDefaultAsync(employee =>
            employee.EmployeeId.Equals(employeeId));

        if (employee == null)
            throw new ValueNotFoundException($"The employee with identifier {employeeId} does not exist!");

        return employee;
    }

    private static Employee MapDtoToEmployee(EmployeeDto employeeDto, Employee? employee = null)
    {
        var mappedEmployee = employee ?? new Employee();

        mappedEmployee.EmployeeId = employee is not null ? mappedEmployee.EmployeeId : Guid.NewGuid();
        mappedEmployee.FirstName = employeeDto.FirstName;
        mappedEmployee.LastName = employeeDto.LastName;
        mappedEmployee.DateOfBirth = employeeDto.DateOfBirth.ToUniversalTime();
        mappedEmployee.Age = employeeDto.Age;
        mappedEmployee.Address = employeeDto.Address;
        mappedEmployee.Bonus = employeeDto.Bonus;
        mappedEmployee.Email = employeeDto.Email;
        mappedEmployee.PhoneNumber = employeeDto.PhoneNumber;
        mappedEmployee.Salary = employeeDto.Salary;
        mappedEmployee.Contract = employeeDto.Contract;
        mappedEmployee.IsOwner = employeeDto.IsOwner;

        return mappedEmployee;
    }
}