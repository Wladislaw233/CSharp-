using Models;

namespace PracticeWithTypes
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Person person = new Person { FirstName = "Ivan", LastName = "Ivanov", DateOfBirth = DateTime.Now};
            Employee employee = new Employee { Person = person};
            UpdateEmployeeContract(employee);
            Console.WriteLine("{0}", employee.Contract);
            Currency currency = new Currency();
            ChangeCurrency(ref currency);
            Console.WriteLine("Валюта: {0} {1}", currency.Name, currency.Course);
            Console.ReadLine();
        }
        static void ChangeCurrency(ref Currency currency)
        {
            currency.Name = "RUB";
            currency.Course = 16.35;
        }
        static void UpdateEmployeeContract(Employee employee)
        {
            employee.Contract = "Контракт сотрудника: " 
                                + employee.Person.FirstName + " " 
                                + employee.Person.LastName + ", дата рождения: " + employee.Person.DateOfBirth.ToString();
        }
    }    
}