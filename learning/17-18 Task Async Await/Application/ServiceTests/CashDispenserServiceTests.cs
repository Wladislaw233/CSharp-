using BankingSystemServices.Database;
using Services;

namespace ServiceTests;

public class CashDispenserServiceTests
{
    public static async Task CashDispenserServiceTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Обналичивание счетов клиентов. (16)");
        Console.ResetColor();
        
        await using (var bankingSystemDbContext = new BankingSystemDbContext())
        {
            var clientService = new ClientService(bankingSystemDbContext);
            var bankClients = clientService.ClientsWithFilterAndPagination(1, 5).Result;
            
            var tasks = new List<Task>();
            
            foreach (var client in bankClients)
            {
                Console.WriteLine($"Лицевые счита клиента {client.FirstName} {client.LastName} до обналичивания:");
                Console.WriteLine(clientService.GetPresentationClientAccounts(client.ClientId));
                
                var task = CashDispenserService.CashOutAsync(client);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            Console.WriteLine();
            
            foreach (var client in bankClients)
            {
                Console.WriteLine($"Лицевые счита клиента {client.FirstName} {client.LastName} после обналичивания:");
                Console.WriteLine(clientService.GetPresentationClientAccounts(client.ClientId));
            }
        }
    }
}