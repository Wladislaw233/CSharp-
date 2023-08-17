using Models;
using Services.Exceptions;
using Services.Storage;

namespace Services;

public class EmployeeService
{
    public readonly List<Employee> BankEmployees = new();

    public void AddBankEmployee(Employee employee)
    {
        ValidationEmployee(employee);
        BankEmployees.Add(employee);
    }

    private void ValidationEmployee(Employee employee)
    {
        if (BankEmployees.Contains(employee))
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

        if (CalculateAge(employee.DateOfBirth) < 18)
            throw new CustomException("сотрудника меньше 18 лет!", nameof(employee.Age));

        if (CalculateAge(employee.DateOfBirth) != employee.Age || employee.Age <= 0)
        {
            employee.Age = CalculateAge(employee.DateOfBirth);
            Console.WriteLine("Возраст сотрудника указан неверно и был скорректирован по дате его рождения!");
        }
    }

    private int CalculateAge(DateTime dateOfBirth)
    {
        return DateTime.Now.Year - dateOfBirth.Year - (dateOfBirth >
                                                       DateTime.Now.AddYears(-(DateTime.Now.Year - dateOfBirth.Year))
            ? 1
            : 0);
    }
    
    public static IEnumerable<Employee> GetEmployeesByFilters(EmployeeStorage employeeStorage, string firstNameFilter = "",
        string lastNameFilter = "", string phoneNumberFilter = "", string contractFilter = "", int? salaryFilter = null, DateTime? minDateOfBirth = null,
        DateTime? maxDateOfBirth = null)
    {
        IEnumerable<Employee> filteredEmployees = employeeStorage;
        if (!string.IsNullOrWhiteSpace(firstNameFilter))
            filteredEmployees = filteredEmployees.Where(employee  => employee.FirstName.Contains(firstNameFilter));
        if (!string.IsNullOrWhiteSpace(lastNameFilter))
            filteredEmployees = filteredEmployees.Where(employee  => employee.LastName.Contains(lastNameFilter));
        if (!string.IsNullOrWhiteSpace(phoneNumberFilter))
            filteredEmployees = filteredEmployees.Where(employee  => employee.PhoneNumber.Contains(phoneNumberFilter));
        if (!string.IsNullOrWhiteSpace(contractFilter))
            filteredEmployees = filteredEmployees.Where(employee  => employee.Contract.Contains(contractFilter));
        if (salaryFilter.HasValue)
            filteredEmployees = filteredEmployees.Where(employee  => employee.Salary == salaryFilter);
        if (minDateOfBirth.HasValue)
            filteredEmployees = filteredEmployees.Where(employee  => employee.DateOfBirth >= minDateOfBirth);
        if (maxDateOfBirth.HasValue)
            filteredEmployees = filteredEmployees.Where(employee  => employee.DateOfBirth <= maxDateOfBirth);
        return filteredEmployees;
    }
}