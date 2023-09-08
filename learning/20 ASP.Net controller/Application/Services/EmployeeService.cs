using BankingSystemServices.Models;
using BankingSystemServices.Services;
using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class EmployeeService
{
    private BankingSystemDbContext _bankingSystemDbContext;

    public EmployeeService(BankingSystemDbContext bankingSystemDbContext)
    {
        _bankingSystemDbContext = bankingSystemDbContext;
        if (!_bankingSystemDbContext.Database.CanConnect())
            throw new CustomException("Failed to establish a connection to the database!");
    }

    public async Task<Employee> AddEmployee(EmployeeDto employeeDto)
    {
        var employee = MapDtoToEmployee(employeeDto);
        
        await ValidateEmployee(employee);
        await _bankingSystemDbContext.Employees.AddAsync(employee);
        await SaveChanges();
        
        return employee;
    }

    public async Task<Employee> UpdateEmployee(Guid employeeId, EmployeeDto employeeDto)
    {
        var employee =
            await _bankingSystemDbContext.Employees.SingleOrDefaultAsync(employee => employee.EmployeeId.Equals(employeeId));

        if (employee == null)
            throw new CustomException($"The employee with identifier {employeeId} does not exist!",
                nameof(employeeId));

        employee = MapDtoToEmployee(employeeDto, employee);
        
        await ValidateEmployee(employee, true);
        _bankingSystemDbContext.Employees.Update(employee);
        await SaveChanges();

        return employee;
    }

    public async Task DeleteEmployee(Guid employeeId)
    {
        var bankEmployee =
            await _bankingSystemDbContext.Employees.SingleOrDefaultAsync(employee => employee.EmployeeId.Equals(employeeId));
        
        if (bankEmployee == null)
            throw new CustomException($"The employee with identifier {employeeId} does not exist!", nameof(employeeId));
        
        _bankingSystemDbContext.Employees.Remove(bankEmployee);
        
        await SaveChanges();
    }

    private async Task ValidateEmployee(Employee employee, bool isUpdate = false)
    {
        if (!isUpdate && await _bankingSystemDbContext.Employees.ContainsAsync(employee))
            throw new CustomException("This employee has already been added to the banking system!", nameof(employee));
        if (string.IsNullOrWhiteSpace(employee.FirstName))
            throw new CustomException("The employee first name is not specified!", nameof(employee.FirstName));
        if (string.IsNullOrWhiteSpace(employee.LastName))
            throw new CustomException("The employee last name is not specified!", nameof(employee.LastName));
        if (string.IsNullOrWhiteSpace(employee.PhoneNumber))
            throw new CustomException("The employee phone number is not specified!", nameof(employee.PhoneNumber));
        if (string.IsNullOrWhiteSpace(employee.Email))
            throw new CustomException("The employee e-mail is not specified!", nameof(employee.Email));
        if (string.IsNullOrWhiteSpace(employee.Address))
            throw new CustomException("The employee address is not specified!", nameof(employee.Address));
        if (employee.Salary == 0)
            throw new CustomException("The employee salary is not specified!", nameof(employee.Salary));
        if (string.IsNullOrWhiteSpace(employee.Contract))
        {
            employee.Contract = $"{employee.FirstName} {employee.LastName}, date of birth: {employee.DateOfBirth}";
        }

        if (employee.DateOfBirth > DateTime.Now || employee.DateOfBirth == DateTime.MinValue ||
            employee.DateOfBirth == DateTime.MaxValue)
            throw new CustomException("The employee's date of birth is incorrect!", nameof(employee.DateOfBirth));

        var age = TestDataGenerator.CalculateAge(employee.DateOfBirth);

        if (age < 18)
            throw new CustomException("Employee is under 18 years old!", nameof(employee.Age));

        if (age != employee.Age || employee.Age <= 0)
        {
            employee.Age = age;
        }
    }

    public async Task<Employee?> GetEmployeeById(Guid employeeId)
    {
        return await _bankingSystemDbContext.Employees.SingleOrDefaultAsync(employee =>
            employee.EmployeeId.Equals(employeeId));
    }
    
    private async Task SaveChanges()
    {
        try
        {
            await _bankingSystemDbContext.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            throw new CustomException(exception.Message, nameof(_bankingSystemDbContext));
        }
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