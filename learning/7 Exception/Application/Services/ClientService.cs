using Models;

namespace Services;

public class ClientService
{
    private static Dictionary<Client, List<Account>> _clientsAccounts = new Dictionary<Client, List<Account>>();

    public static Client AddClient(string firstName, string lastName, DateTime dateOfBirth, int age, string phoneNumber,
        string email, string address)
    {
        
        return new Client( firstName,  lastName,  dateOfBirth,  age,  phoneNumber,
             email,  address);
    }

    private static void ValidateClient(string firstName, string lastName, DateTime dateOfBirth, ref int age, string phoneNumber,
        string email, string address)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("Не указано имя клиента! - ", nameof(firstName));
        else if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Не указана фамилия клиента! - ", nameof(lastName)); 
        else if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Не указан номер клиента! - ", nameof(phoneNumber));
        else if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Не указан e-mail клиента! - ", nameof(email));
        else if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Не указан адрес клиента! - ", nameof(email));

        if (dateOfBirth > DateTime.Now || dateOfBirth == DateTime.MinValue || dateOfBirth == DateTime.MaxValue)
            throw new ArgumentException("Дата рождения клиента указана неверно! - ", nameof(dateOfBirth));
        
        if (CalculateAge(dateOfBirth) < 18)
            throw new ArgumentException("Клиенту меньше 18 лет! - ", nameof(age));
        
        if (CalculateAge(dateOfBirth) != age || age <= 0)
        {
            age = CalculateAge(dateOfBirth);
            Console.WriteLine("Возраст клиента указан неверно и был скорректирован по дате его рождения!");
        }
    }
    private static int CalculateAge(DateTime dateOfBirth)
    {
        return DateTime.Now.Year - dateOfBirth.Year - (dateOfBirth >
                                                       DateTime.Now.AddYears(-(DateTime.Now.Year - dateOfBirth.Year))
                                                       ? 1
                                                       : 0);
    }
}