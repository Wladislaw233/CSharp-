using Models;

namespace Services.Storage;

public class EmployeeStorage : IEmployeeStorage
{
    public List<Employee> Data { get; } = new();

    public void Add(Employee employee)
    {
        EmployeeService.ValidationEmployee(Data,employee);
        Data.Add(employee);
    }

    public void Update(Employee oldEmployee, Employee newEmployee)
    {
        EmployeeService.ValidationEmployee(Data, newEmployee);
        EmployeeService.BeforeDeletingEmployee(Data, oldEmployee);
        Data.Remove(oldEmployee);
        Data.Add(newEmployee);
    }

    public void Delete(Employee employee)
    {
        EmployeeService.BeforeDeletingEmployee(Data, employee);
        Data.Remove(employee);
    }
}