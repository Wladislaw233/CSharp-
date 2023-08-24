using Models;
using Services.Database;
using Services.Exceptions;

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

    public void AddEmployee(Employee employee)
    {
        ValidateEmployee(employee);
        _bankingSystemDbContext.Employees.Add(employee);
        SaveChanges();
    }

    public void UpdateEmployee(Guid employeeId, string? firstName = null, string? lastName = null, int? age = null,
        DateTime? dateOfBirth = null, string? phoneNumber = null, string? address = null, string? email = null,
        string? contract = null, double? salary = null, bool? isOwner = null)
    {
        var employee =
            _bankingSystemDbContext.Employees.SingleOrDefault(employee => employee.EmployeeId.Equals(employeeId));

        if (employee == null)
            throw new CustomException($"Клиента с идентификатором {employeeId} не существует!",
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
            employee.Salary = (double)salary;
        if (isOwner != null)
            employee.IsOwner = (bool)isOwner;

        ValidateEmployee(employee, true);
        _bankingSystemDbContext.Employees.Update(employee);
        SaveChanges();
    }

    public void DeleteEmployee(Guid employeeId)
    {
        var bankEmployee =
            _bankingSystemDbContext.Employees.SingleOrDefault(employee => employee.EmployeeId.Equals(employeeId));
        if (bankEmployee == null)
            throw new CustomException($"Клиента с идентификатором {employeeId} не существует!", nameof(employeeId));
        _bankingSystemDbContext.Employees.Remove(bankEmployee);
        SaveChanges();
    }

    private void ValidateEmployee(Employee employee, bool isUpdate = false)
    {
        if (!isUpdate && _bankingSystemDbContext.Employees.Contains(employee))
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
    
    private void SaveChanges()
    {
        try
        {
            _bankingSystemDbContext.SaveChanges();
        }
        catch (Exception exception)
        {
            throw new CustomException(exception.Message, nameof(_bankingSystemDbContext));
        }
    }
    
    public List<Employee> EmployeesWithFilterAndPagination(int page, int pageSize, string? firstName = null,
        string? lastName = null, int? age = null,
        DateTime? dateOfBirth = null, string? phoneNumber = null, string? address = null, string? email = null,
        string? contract = null, double? salary = null, bool? isOwner = null)
    {
        IQueryable<Employee> query = _bankingSystemDbContext.Employees;
        if (firstName != null)
            query = query.Where(employee => employee.FirstName.Contains(firstName));
        if (lastName != null)
            query = query.Where(employee => employee.LastName.Contains(lastName));
        if (age != null)
            query = query.Where(employee => employee.Age.Equals((int)age));
        if (dateOfBirth != null)
            query = query.Where(employee => employee.DateOfBirth.Equals(((DateTime)dateOfBirth).ToUniversalTime()));
        if (phoneNumber != null)
            query = query.Where(employee => employee.PhoneNumber.Contains(phoneNumber));
        if (address != null)
            query = query.Where(employee => employee.Address.Contains(address));
        if (email != null)
            query = query.Where(employee => employee.Email.Contains(email));
        if (contract != null)
            query = query.Where(employee => employee.Contract.Contains(contract));
        if (salary != null)
            query = query.Where(employee => employee.Salary.Equals(salary));
        if (isOwner != null)
            query = query.Where(employee => employee.IsOwner.Equals(isOwner));

        query = query.OrderBy(employee => employee.FirstName);

        query = query.Skip((page - 1) * pageSize).Take(pageSize);

        return query.ToList();
    }
}