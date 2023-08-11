
namespace Models
{
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Contract { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Owner { get; set; }
        public int Salary { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // explicit - при явном преобразовании
        // implicit - при не явном преобразовании
        public static explicit operator Employee(Client client)
        {
            DateTime dateOfBirth = DateTime.Now;
            return new Employee{ 
                                FirstName = client.FirstName
                                , LastName = client.LastName
                                , Owner = false
                                , Salary = 0
                                , DateOfBirth = dateOfBirth 
                                , Contract = client.FirstName + "" + client.LastName + ", дата рождения: " +dateOfBirth.ToString()
                                , Address = client.Address
                                , PhoneNumber = client.PhoneNumber
                                , Email = client.Email};
        }
    }
}