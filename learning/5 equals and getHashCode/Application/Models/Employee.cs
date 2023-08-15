namespace Models
{
    public class Employee : Person
    {
        
        public string Contract { get; init; }
        public int Salary { get; init; }
        public string Address { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }

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