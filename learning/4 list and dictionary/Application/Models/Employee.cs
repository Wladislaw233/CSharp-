namespace Models
{
    public class Employee : Person
    {
        
        public string Contract { get; set; }
        public int Salary { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public Employee(string firstName, string lastName, DateTime dateOfBirth, int age, string contract, int salary = 0, string address = "", string email = "", string phoneNumber = "")
            :base(firstName, lastName, dateOfBirth, age)
        {
            Contract = contract;
            Salary = salary;
            Address = address;
            Email = email;
            PhoneNumber = phoneNumber;
        }
        
        // explicit - при явном преобразовании
        // implicit - при не явном преобразовании
        public static explicit operator Employee(Client client)
        {
            return new Employee(
                client.FirstName,
                client.LastName,
                client.DateOfBirth,
                client.Age,
                client.FirstName + " " + client.LastName + ", дата рождения: " + client.DateOfBirth.ToString(),
                0,
                client.Address,
                client.Email,
                client.PhoneNumber
            );
        }
    }
}