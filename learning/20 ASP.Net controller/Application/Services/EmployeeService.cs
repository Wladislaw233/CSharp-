using BankingSystemServices.Models;
using BankingSystemServices.Services;
using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class EmployeeService
{
    private BankingSystemDbContext _bankingSystemDbContext;

    public EmployeeService(BankingSystemDbContext bankingSystemDbContext)
    {
        _bankingSystemDbContext = bankingSystemDbContext;
        if (!_bankingSystemDbContext.Database.CanConnect())
            throw new CustomException("Не удалось установить соединение с базой данных!");
    }

    public async Task AddEmployee(Employee employee)
    {
        await ValidateEmployee(employee);
        await _bankingSystemDbContext.Employees.AddAsync(employee);
        await SaveChanges();
    }

    public async Task UpdateEmployee(Guid employeeId, string? firstName = null, string? lastName = null, int? age = null,
        DateTime? dateOfBirth = null, string? phoneNumber = null, string? address = null, string? email = null,
        string? contract = null, decimal? salary = null, bool? isOwner = null, decimal? bonus = null)
    {
        var employee =
            await _bankingSystemDbContext.Employees.SingleOrDefaultAsync(employee => employee.EmployeeId.Equals(employeeId));

        if (employee == null)
            throw new CustomException($"Сотрудника с идентификатором {employeeId} не существует!",
                nameof(employeeId));
        if (firstName != null)
            employee.FirstName = firstName;
        if (lastName != null)
            employee.LastName = lastName;
        if (age != null)
            employee.Age = (int)age;
        if (dateOfBirth != null)
            employee.DateOfBirth = ((DateTime)dateOfBirth).ToUniversalTime();
        if (phoneNumber != null)
            employee.PhoneNumber = phoneNumber;
        if (address != null)
            employee.Address = address;
        if (email != null)
            employee.Email = email;
        if (contract != null)
            employee.Contract = contract;
        if (salary != null)
            employee.Salary = (decimal)salary;
        if (isOwner != null)
            employee.IsOwner = (bool)isOwner;
        if (bonus != null)
            employee.Bonus = (decimal)bonus;

        await ValidateEmployee(employee, true);
        _bankingSystemDbContext.Employees.Update(employee);
        await SaveChanges();
    }

    public async Task DeleteEmployee(Guid employeeId)
    {
        var bankEmployee =
            await _bankingSystemDbContext.Employees.SingleOrDefaultAsync(employee => employee.EmployeeId.Equals(employeeId));
        if (bankEmployee == null)
            throw new CustomException($"Сотрудника с идентификатором {employeeId} не существует!", nameof(employeeId));
        _bankingSystemDbContext.Employees.Remove(bankEmployee);
        await SaveChanges();
    }

    private async Task ValidateEmployee(Employee employee, bool isUpdate = false)
    {
        if (!isUpdate && await _bankingSystemDbContext.Employees.ContainsAsync(employee))
            throw new CustomException("Данный сотрудник уже добавлен в банковскую систему!", nameof(employee));
        if (string.IsNullOrWhiteSpace(employee.FirstName))
            throw new CustomException("Не указано имя сотрудника!", nameof(employee.FirstName));
        if (string.IsNullOrWhiteSpace(employee.LastName))
            throw new CustomException("Не указана фамилия сотрудника!", nameof(employee.LastName));
        if (string.IsNullOrWhiteSpace(employee.PhoneNumber))
            throw new CustomException("Не указан номер сотрудника!", nameof(employee.PhoneNumber));
        if (string.IsNullOrWhiteSpace(employee.Email))
            throw new CustomException("Не указан e-mail сотрудника!", nameof(employee.Email));
        if (string.IsNullOrWhiteSpace(employee.Address))
            throw new CustomException("Не указан адрес сотрудника!", nameof(employee.Email));
        if (employee.Salary == 0)
            throw new CustomException("Не указана зарплата сотрудника!", nameof(employee.Salary));
        if (string.IsNullOrWhiteSpace(employee.Contract))
        {
            employee.Contract = $"{employee.FirstName} {employee.LastName}, дата рождения: {employee.DateOfBirth}";
            Console.WriteLine("Контракт сотрудника был создан!");
        }

        if (employee.DateOfBirth > DateTime.Now || employee.DateOfBirth == DateTime.MinValue ||
            employee.DateOfBirth == DateTime.MaxValue)
            throw new CustomException("Дата рождения сотрудника указана неверно!", nameof(employee.DateOfBirth));

        var age = TestDataGenerator.CalculateAge(employee.DateOfBirth);

        if (age < 18)
            throw new CustomException("сотрудника меньше 18 лет!", nameof(employee.Age));

        if (age != employee.Age || employee.Age <= 0)
        {
            employee.Age = age;
            Console.WriteLine("Возраст сотрудника указан неверно и был скорректирован по дате его рождения!");
        }
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
    
    public async Task<List<Employee>> EmployeesWithFilterAndPagination(int page, int pageSize, string? firstName = null,
        string? lastName = null, int? age = null, Guid? employeeId = null,
        DateTime? dateOfBirth = null, string? phoneNumber = null, string? address = null, string? email = null,
        string? contract = null, double? salary = null, bool? isOwner = null)
    {
        IQueryable<Employee> query = _bankingSystemDbContext.Employees;
        if (firstName != null)
            query = query.Where(employee => employee.FirstName == firstName);
        if (lastName != null)
            query = query.Where(employee => employee.LastName== lastName);
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