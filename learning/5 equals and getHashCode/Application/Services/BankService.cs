using System.Collections.Generic;
using Models;

namespace Services
{
    public class BankService
    {
        public void CalculateSalary(double profit, double expenses, List<Employee> employees)
        {
            var salary = (profit - expenses) / employees.Count;
            foreach (var employee in employees) 
                employee.Salary = (int)salary;
        }
    }
}