using Models;

namespace Services;

public class TestDataGenerator
{
    private static Random random = new Random();
    private static List<string> names = new List<string>
    {
        "Eli", "Lucius", "Stephan", "Kirby", "Barbra", "Ralph", "Karla",
        "Taylor", "Benita", "Lisa", "Roy", "Buck", "Rod", "Colette", "Quinn"
    };
    private static List<string> surnames = new List<string>
    {
        "Ali", "Stout", "Christian", "Bright", "Shelton", "Tapia", "Carr",
        "Bryant", "Romero", "Zhang", "Barton", "Paul", "Berger", "Chandler", "Collins"
    };
    private static List<string> streets = new List<string>
    {
        ", ул. Ленина", ", ул. Мира", ", ул. 25 Октября", ", ул. Мая", ", ул. Лесная", ", пер. Энгельса"
    };
    private static List<string> cities = new List<string>
    {
        "Тирасполь", "Рыбница", "Григориополь", "Дубоссары", "Каменка"
    };
    private static List<string> emailDomains = new List<string>
    {
        "@gmail.com", "@yahoo.com", "@outlook.com", "@hotmail.com", "@example.com", "@testmail.com"
    };
    private static string GenerateRandomPhoneNumber()
    {
        return string.Format("{0:8}",random.Next(10000000, 99999999));
    }
    private static string GenerateRandomFirstName()
    {
        return names[random.Next(names.Count)];
    }
    private static string GenerateRandomLastName()
    {
        return surnames[random.Next(surnames.Count)];
    }
    private static string GenerateRandomEmail(string firstName, string lastName)
    {
        return firstName + lastName + emailDomains[random.Next(emailDomains.Count)];
    }
    private static string GenerateRandomAddress()
    {
        return cities[random.Next(cities.Count)] + streets[random.Next(streets.Count)];
    }
    private static DateTime GenerateRandomDateOfBirth()
    {
        int year = random.Next(1900, DateTime.Now.Year - 18);
        int month = random.Next(1, 13);
        int day = random.Next(1, DateTime.DaysInMonth(year, month) + 1);
        return new DateTime(year, month, day);
    }
    public static List<Client> GenerateListWitchThousandBankClients()
    {
        List<Client> listBankClients = new List<Client>();
        string firstName;
        string lastName;
        for (int index = 0; index < 1000; index++)
        {
            firstName = GenerateRandomFirstName();
            lastName = GenerateRandomLastName();
            listBankClients.Add(new Client
            {
                FirstName = firstName,
                LastName = lastName,
                Address = GenerateRandomAddress(), 
                Email = GenerateRandomEmail(firstName, lastName),
                PhoneNumber = GenerateRandomPhoneNumber()
            });
        }
        return listBankClients;
    }

    public static Dictionary<string, Client> GenerateDictionaryWithBankClients(List<Client> listBankClients)
    {
        Dictionary<string, Client> dictionaryBankClients = new Dictionary<string, Client>();
        foreach (var bankClient in listBankClients) dictionaryBankClients[bankClient.PhoneNumber] = bankClient;
        return dictionaryBankClients;
    }

    public static List<Employee> GenerateListWithThousandEmployers()
    {
        List<Employee> listBankEmployers = new List<Employee>();
        string firstName;
        string lastName;
        DateTime dateOfBirth;
        for (int index = 0; index < 1000; index++)
        {
            firstName = GenerateRandomFirstName();
            lastName = GenerateRandomLastName();
            dateOfBirth = GenerateRandomDateOfBirth();
            listBankEmployers.Add(new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Address = GenerateRandomAddress(),
                DateOfBirth = dateOfBirth,
                Contract = firstName + " " + lastName + ", дата рождения: " + dateOfBirth,
                Email = GenerateRandomEmail(firstName, lastName),
                Owner = false,
                PhoneNumber = GenerateRandomPhoneNumber(),
                Salary = random.Next(10000, 99999)
            });
        }
        return listBankEmployers;
    }
}