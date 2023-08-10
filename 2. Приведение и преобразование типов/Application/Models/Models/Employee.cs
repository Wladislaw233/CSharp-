namespace Models
{
    public class Employee
    {
        public Person Person { get; set; }
        public string Contract { get; set; }
        public bool Owner { get; set; }
        public int Salary { get; set; }
    }
}