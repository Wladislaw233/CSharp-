
namespace Models
{
    public class Employee
    {
        public Person Person { get; set; }
        public string Contract { get; set; }
        public bool Owner { get; set; }
        public int Salary { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // explicit - при явном преобразовании
        // implicit - при не явном преобразовании
        public static explicit operator Employee(Client client)
        {
            Person person = new Person();
            person.FirstName = client.FirstName;
            person.LastName = client.LastName;
            person.DateOfBirth = DateTime.Now;
            return new Employee{ Person = person
                                , Owner = false
                                , Salary = 0
                                , Contract = person.FirstName + "" + person.LastName + ", дата рождения: " + person.DateOfBirth.ToString()
                                , Address = client.Address
                                , PhoneNumber = client.PhoneNumber
                                , Email = client.Email};
        }
    }
}