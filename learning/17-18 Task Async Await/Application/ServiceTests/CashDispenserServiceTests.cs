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
        var bankClients = clientService.ClientsWithFilterAndPagination(1, 5).Result;

        var tasks = new List<Task>();

        foreach (var client in bankClients)
        {
            Console.WriteLine(
                $"Personal accounts of the client {client.FirstName} {client.LastName} before cashing out:");

            var presentationClientAccounts = await clientService.GetPresentationClientAccounts(client.ClientId);

            Console.WriteLine(presentationClientAccounts);

            var task = CashDispenserService.CashOutAsync(client);
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);

        Console.WriteLine();

        foreach (var client in bankClients)
        {
            Console.WriteLine($"Personal accounts of client {client.FirstName} {client.LastName} after cashing out:");
            var presentationClientAccounts = await clientService.GetPresentationClientAccounts(client.ClientId);
            Console.WriteLine(presentationClientAccounts);
        }
    }
}