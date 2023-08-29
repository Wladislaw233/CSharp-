using System.Collections;
using BankingSystemServices;

namespace Services.Storage;

public class EmployeeStorage : IEnumerable<Employee>
{
    private readonly List<Employee> _bankEmployees = new();

    public void AddBankEmployees(Employee employee)
    {
        _bankEmployees.Add(employee);
    }

    public IEnumerator<Employee> GetEnumerator()
    {
        return _bankEmployees.GetEnumerator();
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}