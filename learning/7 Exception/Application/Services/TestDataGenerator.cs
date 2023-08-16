using Models;

namespace Services;

public class TestDataGenerator
{
    private static readonly Random Rnd = new();

    private static readonly List<string> Names = new()
    {
        "Eli", "Lucius", "Stephan", "Kirby", "Barbra", "Ralph", "Karla",
        "Taylor", "Benita", "Lisa", "Roy", "Buck", "Rod", "Colette", "Quinn"
    };

    private static readonly List<string> Surnames = new()
    {
        "Ali", "Stout", "Christian", "Bright", "Shelton", "Tapia", "Carr",
        "Bryant", "Romero", "Zhang", "Barton", "Paul", "Berger", "Chandler", "Collins"
    };

    private static readonly List<string> Streets = new()
    {
        ", ул. Ленина", ", ул. Мира", ", ул. 25 Октября", ", ул. Мая", ", ул. Лесная", ", пер. Энгельса"
    };

    private static readonly List<string> Cities = new()
    {
        "Тирасполь", "Рыбница", "Григориополь", "Дубоссары", "Каменка"
    };

    private static readonly List<string> EmailDomains = new()
    {
        "@gmail.com", "@yahoo.com", "@outlook.com", "@hotmail.com", "@example.com", "@testmail.com"
    };

    private static string GenerateRandomPhoneNumber()
    {
        return string.Format("{0:8}", Rnd.Next(10000000, 99999999));
    }

    private static string GenerateRandomFirstName()
    {
        return Names[Rnd.Next(Names.Count)];
    }

    private static string GenerateRandomLastName()
    {
        return Surnames[Rnd.Next(Surnames.Count)];
    }

    private static string GenerateRandomEmail(string firstName, string lastName)
    {
        return firstName + lastName + EmailDomains[Rnd.Next(EmailDomains.Count)];
    }

    private static string GenerateRandomAddress()
    {
        return Cities[Rnd.Next(Cities.Count)] + Streets[Rnd.Next(Streets.Count)];
    }

    private static DateTime GenerateRandomDateOfBirth()
    {
        var year = Rnd.Next(1950, DateTime.Now.Year - 18);
        var month = Rnd.Next(1, 13);
        var day = Rnd.Next(1, DateTime.DaysInMonth(year, month) + 1);
        return new DateTime(year, month, day);
    }

    private static int CalculateAge(DateTime dateOfBirth)
    {
        return DateTime.Now.Year - dateOfBirth.Year - (dateOfBirth >
                                                       DateTime.Now.AddYears(-(DateTime.Now.Year - dateOfBirth.Year))
            ? 1
            : 0);
    }

    private static Client GenerateRandomClient()
    {
        var firstName = GenerateRandomFirstName();
        var lastName = GenerateRandomLastName();
        var dateOfBirth = GenerateRandomDateOfBirth();
        return new Client
        (
            firstName,
            lastName,
            dateOfBirth,
            CalculateAge(dateOfBirth),
            GenerateRandomPhoneNumber(),
            GenerateRandomEmail(firstName, lastName),
            GenerateRandomAddress()
        );
    }

    private static Employee GenerateRandomEmployee()
    {
        var firstName = GenerateRandomFirstName();
        var lastName = GenerateRandomLastName();
        var dateOfBirth = GenerateRandomDateOfBirth();
        return new Employee
        (firstName,
            lastName,
            dateOfBirth,
            CalculateAge(dateOfBirth),
            firstName + " " + lastName + ", дата рождения: " + dateOfBirth,
            Rnd.Next(10000, 99999),
            GenerateRandomAddress(),
            GenerateRandomEmail(firstName, lastName),
            GenerateRandomPhoneNumber());
    }

    private static Account GenerateRandomAccount()
    {
        return new Account("123456789");
    }

    public static List<Client> GenerateListWitchBankClients(int numberOfClients = 10, bool addInvalidClient = false)
    {
        var listBankClients = new List<Client>();
        for (var index = 0; index < numberOfClients; index++)
            listBankClients.Add(GenerateRandomClient());
        if (addInvalidClient)
            AddInvalidClientToList(ref listBankClients);
        return listBankClients;
    }

    private static void AddInvalidClientToList(ref List<Client> listBankClients)
    {
        var firstName = "";
        var lastName = "";
        var dateOfBirth = GenerateRandomDateOfBirth();
        listBankClients.Add(new Client
        (
            firstName,
            lastName,
            dateOfBirth,
            CalculateAge(dateOfBirth),
            GenerateRandomPhoneNumber(),
            GenerateRandomEmail(firstName, lastName),
            GenerateRandomAddress()
        ));
    }

    public static Dictionary<string, Client> GenerateDictionaryWithBankClients(List<Client> listBankClients)
    {
        var dictionaryBankClients = new Dictionary<string, Client>();
        foreach (var bankClient in listBankClients)
            dictionaryBankClients[bankClient.PhoneNumber] = bankClient;
        dictionaryBankClients.Add("00000000", GenerateRandomClient());
        return dictionaryBankClients;
    }

    public static List<Employee> GenerateListWithEmployees(int numberOfEmployees = 10, bool addInvalidEmployee = false)
    {
        var listBankEmployees = new List<Employee>();
        for (var index = 0; index < numberOfEmployees; index++)
            listBankEmployees.Add(GenerateRandomEmployee());
        if (addInvalidEmployee)
            AddInvalidEmployeeToList(ref listBankEmployees);
        return listBankEmployees;
    }

    private static void AddInvalidEmployeeToList(ref List<Employee> listBankEmployees)
    {
        var firstName = "";
        var lastName = "";
        var dateOfBirth = GenerateRandomDateOfBirth();
        listBankEmployees.Add(new Employee
        (firstName,
            lastName,
            dateOfBirth,
            CalculateAge(dateOfBirth),
            firstName + " " + lastName + ", дата рождения: " + dateOfBirth,
            Rnd.Next(10000, 99999),
            GenerateRandomAddress(),
            GenerateRandomEmail(firstName, lastName),
            GenerateRandomPhoneNumber()));
    }

    public static Dictionary<Client, List<Account>> GenerateDictionaryWithClientsAccounts(int numberOfClients = 10)
    {
        var clientsAccounts = new Dictionary<Client, List<Account>>();
        var listBankClients = GenerateListWitchBankClients(numberOfClients);
        foreach (var bankClient in listBankClients)
            clientsAccounts.TryAdd(bankClient, new List<Account> { GenerateRandomAccount() });
        return clientsAccounts;
    }

    public static void AddClientAccount(ref Dictionary<Client, List<Account>> clientsAccounts, Client client)
    {
        if (clientsAccounts.ContainsKey(client))
            clientsAccounts[client].Add(GenerateRandomAccount());
    }
}