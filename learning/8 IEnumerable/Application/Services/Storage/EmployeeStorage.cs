using System.Collections;
using Models;

namespace Services.Storage;

public class EmployeeStorage : IEnumerable<Employee>
{
    private readonly List<Employee> _bankEmployees = new();

    public void AddBankEmployees(int numberOfClients = 10)
    {
        _bankEmployees.AddRange(TestDataGenerator.GenerateListWithEmployees(numberOfClients));
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