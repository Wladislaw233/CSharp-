using BankingSystemServices.Database;
using Services;

namespace ServiceTests;

public static class CashDispenserServiceTests
{
    public static async Task CashDispenserServiceTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Cashing out customer accounts. (16)");
        Console.ResetColor();

        await using var bankingSystemDbContext = new BankingSystemDbContext();

        var clientService = new ClientService(bankingSystemDbContext);

        var bankClients = await clientService.ClientsWithFilterAndPaginationAsync(1, 5);

        var tasks = new List<Task>();

        foreach (var client in bankClients)
        {
            Console.WriteLine(
                $"Personal accounts of the client {client.FirstName} {client.LastName} before cashing out:");

            var mess = await clientService.GetPresentationClientAccountsAsync(client.ClientId);

            Console.WriteLine(mess);

            tasks.Add(CashDispenserService.CashOutAsync(client.ClientId));
        }

        await Task.WhenAll(tasks);

        foreach (var client in bankClients)
        {
            Console.WriteLine($"Personal accounts of client {client.FirstName} {client.LastName} after cashing out:");

            var presentationClientAccounts = await clientService.GetPresentationClientAccountsAsync(client.ClientId);

            Console.WriteLine(presentationClientAccounts);
        }
    }
}