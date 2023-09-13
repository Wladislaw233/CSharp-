using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Services;

namespace Services;

public class EmployeeService
{
    private readonly BankingSystemDbContext _bankingSystemDbContext;

    public EmployeeService(BankingSystemDbContext bankingSystemDbContext)
    {
        _bankingSystemDbContext = bankingSystemDbContext;
        if (!_bankingSystemDbContext.Database.CanConnect())
            throw new DatabaseNotConnectedException("Failed to establish a connection to the database!");
    }

    public void AddEmployee(Employee employee)
    {
        ValidateEmployee(employee);
        _bankingSystemDbContext.Employees.Add(employee);
        SaveChanges();
    }

    public void UpdateEmployee(Guid employeeId, string? firstName = null, string? lastName = null, int? age = null,
        DateTime? dateOfBirth = null, string? phoneNumber = null, string? address = null, string? email = null,
        string? contract = null, decimal? salary = null, bool? isOwner = null, decimal? bonus = null)
    {
        var employee =
            _bankingSystemDbContext.Employees.SingleOrDefault(employee => employee.EmployeeId.Equals(employeeId));

        if (employee == null)
            throw new ArgumentException($"The employee with identifier {employeeId} does not exist!",
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

        ValidateEmployee(employee, true);
        _bankingSystemDbContext.Employees.Update(employee);
        SaveChanges();
    }

    public void DeleteEmployee(Guid employeeId)
    {
        var bankEmployee =
            _bankingSystemDbContext.Employees.SingleOrDefault(employee => employee.EmployeeId.Equals(employeeId));

        if (bankEmployee == null)
            throw new ArgumentException($"The employee with identifier {employeeId} does not exist!",
                nameof(employeeId));

        _bankingSystemDbContext.Employees.Remove(bankEmployee);

        SaveChanges();
    }

    private void ValidateEmployee(Employee employee, bool isUpdate = false)
    {
        if (!isUpdate && _bankingSystemDbContext.Employees.Contains(employee))
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

    private void SaveChanges()
    {
        _bankingSystemDbContext.SaveChanges();
    }

    public List<Employee> EmployeesWithFilterAndPagination(int page, int pageSize, string? firstName = null,
        string? lastName = null, int? age = null,
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

        query = query.OrderBy(employee => employee.FirstName);

        query = query.Skip((page - 1) * pageSize).Take(pageSize);

        return query.ToList();
    }
}