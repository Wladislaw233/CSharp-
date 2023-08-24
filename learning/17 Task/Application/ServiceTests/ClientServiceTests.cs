using Services;
using Services.Database;
using Services.Exceptions;

namespace ServiceTests;

public class ClientServiceTests
{
    public static void ClientServiceTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Клиенты");
        Console.ResetColor();
        using var bankingSystemDbContext = new BankingSystemDbContext();

        try
        {
            var clientService = new ClientService(bankingSystemDbContext);
            var bankClients = TestDataGenerator.GenerateListWitchBankClients(5);
            foreach (var client in bankClients)
                clientService.AddClient(client);

            var bankClient = bankClients.FirstOrDefault();
            if (bankClient != null)
            {
                var presentationBankClientAccounts =
                    clientService.GetPresentationClientAccounts(bankClient.ClientId).ToList();
                Console.WriteLine($"Лицевые счета клиента {bankClient.FirstName} {bankClient.LastName}:" +
                                  $"\n" + string.Join('\n', presentationBankClientAccounts));
                Console.WriteLine("Добавим счет EUR с балансом 1455,23:");
                clientService.AddClientAccount(bankClient.ClientId, "EUR", 1455.23);
                presentationBankClientAccounts =
                    clientService.GetPresentationClientAccounts(bankClient.ClientId).ToList();
                Console.WriteLine($"Лицевые счета клиента {bankClient.FirstName} {bankClient.LastName}:" +
                                  $"\n" + string.Join('\n', presentationBankClientAccounts));
                Console.WriteLine("Удалим счет EUR с балансом 1455,23:");
                var bankClientAccounts = clientService.GetClientAccounts(bankClient.ClientId);
                clientService.DeleteClientAccount(bankClientAccounts[1].AccountId);
                presentationBankClientAccounts =
                    clientService.GetPresentationClientAccounts(bankClient.ClientId).ToList();
                Console.WriteLine($"Лицевые счета клиента {bankClient.FirstName} {bankClient.LastName}:" +
                                  $"\n" + string.Join('\n', presentationBankClientAccounts));
                Console.WriteLine("Изменим клиенту имя и фамилию:" +
                                  $"\nДо изменения {bankClient.FirstName} {bankClient.LastName}. id {bankClient.ClientId}");
                clientService.UpdateClient(bankClient.ClientId, "Влад", "Юрченко");
                Console.WriteLine(
                    $"После изменения {bankClient.FirstName} {bankClient.LastName}. id {bankClient.ClientId}");
                Console.WriteLine($"Удаление клиента с id - {bankClient.ClientId}");
                clientService.DeleteClient(bankClient.ClientId);

                Console.WriteLine("Выведем клиентов с именем Stephan:");
                var filteredClients = clientService.ClientsWithFilterAndPagination(1, 100, "Stephan");
                Console.WriteLine("Клиенты:\n" + string.Join("\n",
                    filteredClients.Select(client =>
                        $"Имя {client.FirstName}, фамилия {client.LastName}, дата рождения {client.DateOfBirth.ToString("D")}")));
            }
        }
        catch (CustomException exception)
        {
            CustomException.ExceptionHandling("Программа остановлена по причине:", exception);
        }
    }
}