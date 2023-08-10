using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Models
{
    public class BankService
    {
        public void CalculateSalary(double profit, double expenses, List<Employee> employees)
        {
            double Salary = (profit - expenses) / employees.Count;
            foreach (Employee employee in employees)
            {
                employee.Salary = (int)Salary;
            }
        }
    }
}