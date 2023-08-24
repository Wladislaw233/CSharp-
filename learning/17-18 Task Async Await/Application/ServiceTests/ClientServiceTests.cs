using Services;
using Services.Database;
using Services.Exceptions;

namespace ServiceTests;

public class ClientServiceTests
{
    public static async Task ClientServiceTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Клиенты");
        Console.ResetColor();
        await using var bankingSystemDbContext = new BankingSystemDbContext();

        try
        {
            var clientService = new ClientService(bankingSystemDbContext);
            var bankClients = TestDataGenerator.GenerateListWitchBankClients(100);
            foreach (var client in bankClients)
                await clientService.AddClient(client);

            var bankClient = bankClients.FirstOrDefault();
            if (bankClient != null)
            {
                var presentationBankClientAccounts =
                    await clientService.GetPresentationClientAccounts(bankClient.ClientId);
                Console.WriteLine($"Лицевые счета клиента {bankClient.FirstName} {bankClient.LastName}:" +
                                  $"\n" + string.Join('\n', presentationBankClientAccounts));
                Console.WriteLine("Добавим счет EUR с балансом 1455,23:");
                await clientService.AddClientAccount(bankClient.ClientId, "EUR", 1455.23);
                presentationBankClientAccounts =
                    await clientService.GetPresentationClientAccounts(bankClient.ClientId);
                Console.WriteLine($"Лицевые счета клиента {bankClient.FirstName} {bankClient.LastName}:" +
                                  $"\n" + string.Join('\n', presentationBankClientAccounts));
                Console.WriteLine("Удалим счет EUR с балансом 1455,23:");
                var bankClientAccounts = await clientService.GetClientAccounts(bankClient.ClientId);
                await clientService.DeleteClientAccount(bankClientAccounts[1].AccountId);
                presentationBankClientAccounts =
                    await clientService.GetPresentationClientAccounts(bankClient.ClientId);
                Console.WriteLine($"Лицевые счета клиента {bankClient.FirstName} {bankClient.LastName}:" +
                                  $"\n" + string.Join('\n', presentationBankClientAccounts));
                Console.WriteLine("Изменим клиенту имя и фамилию:" +
                                  $"\nДо изменения {bankClient.FirstName} {bankClient.LastName}. id {bankClient.ClientId}");
                await clientService.UpdateClient(bankClient.ClientId, "Влад", "Юрченко");
                Console.WriteLine(
                    $"После изменения {bankClient.FirstName} {bankClient.LastName}. id {bankClient.ClientId}");
                Console.WriteLine($"Удаление клиента с id - {bankClient.ClientId}");
                await clientService.DeleteClient(bankClient.ClientId);

                Console.WriteLine("Выведем клиентов с именем Stephan:");
                var filteredClients = await clientService.ClientsWithFilterAndPagination(1, 100, "Stephan");
                Console.WriteLine("Клиенты:\n" + string.Join("\n",
                    filteredClients.Select(client =>
                        $"Имя {client.FirstName}, фамилия {client.LastName}, дата рождения {client.DateOfBirth.ToString("D")}")));
            }
            var numberOfThreads = 5;
            var tasks = new Task[numberOfThreads];
            bankClients = TestDataGenerator.GenerateListWitchBankClients(numberOfThreads);
            var index = 0;
            var numberOfTask = 1;
            foreach (var client in bankClients)
            {
                tasks[index] = Task.Run(() =>
                {
                    using (var bankingSystemDbContextTasks = new BankingSystemDbContext())
                    {
                        var clientServiceTasks = new ClientService(bankingSystemDbContextTasks);
                        clientServiceTasks.AddClient(client).Wait();
                        Console.WriteLine($"Задача {numberOfTask} добавляет клиента - ID {client.ClientId}, {client.FirstName} {client.LastName}");
                        numberOfTask++;
                    }
                });
                index++;
            }
            await Task.WhenAll(tasks);
        }
        catch (CustomException exception)
        {
            CustomException.ExceptionHandling("Программа остановлена по причине:", exception);
        }
    }
}