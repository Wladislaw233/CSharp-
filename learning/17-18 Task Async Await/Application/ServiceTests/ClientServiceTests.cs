using BankingSystemServices.Database;
using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Services;

namespace ServiceTests;

public class ClientServiceTests
{
    private readonly ClientService _clientService;
    private readonly TestDataGenerator _testDataGenerator = new();

    private Client? _client;

    public ClientServiceTests(BankingSystemDbContext bankingSystemDbContext)
    {
        _clientService = new ClientService(bankingSystemDbContext);
    }

    public async Task ClientServiceTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Clients");
        Console.ResetColor();

        _client = await AddClientTest();

        if (_client != null)
        {
            await AddClientAccountTest();
            await UpdateClientAccountTest();
            await DeleteClientAccountTest();
            await UpdateClientTest();
            await DeleteClientTest();
            await GetClientsWithFilterTest();
        }
        else
        {
            Console.WriteLine("Test client not found!");
        }
    }

    private async Task<Client?> AddClientTest()
    {
        var bankClients = _testDataGenerator.GenerateListWithBankClients(5);

        try
        {
            foreach (var client in bankClients)
                await _clientService.AddClientAsync(client);

            return bankClients.FirstOrDefault();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    private async Task AddClientAccountTest()
    {
        if (_client == null)
            return;

        Console.WriteLine($"Client's personal accounts {_client.FirstName} {_client.LastName}:");

        var representationBankClientAccounts =
            await _clientService.GetPresentationClientAccountsAsync(_client.ClientId);

        Console.WriteLine(representationBankClientAccounts);

        const string currencyCode = "EUR";
        var amount = new decimal(1455.23);

        Console.WriteLine($"Add account {currencyCode} with balance {amount}:");

        try
        {
            await _clientService.AddClientAccountAsync(_client.ClientId, currencyCode, amount);

            representationBankClientAccounts =
                await _clientService.GetPresentationClientAccountsAsync(_client.ClientId);

            Console.WriteLine($"Client's personal accounts {_client.FirstName} {_client.LastName}:");
            Console.WriteLine(representationBankClientAccounts);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task UpdateClientAccountTest()
    {
        if (_client == null)
            return;

        try
        {
            var bankClientAccounts = await _clientService.GetClientAccountsAsync(_client.ClientId);

            var account = bankClientAccounts.LastOrDefault();

            if (account == null)
            {
                Console.WriteLine("Client account for update not found.");
                return;
            }

            var amount = new decimal(30000);

            Console.WriteLine(
                $"Let's update the account balance {account.AccountNumber} for client {_client.FirstName} {_client.LastName} to {amount}:");

            await _clientService.UpdateClientAccountAsync(account.AccountId, amount: amount);

            var presentationBankClientAccounts =
                await _clientService.GetPresentationClientAccountsAsync(_client.ClientId);

            Console.WriteLine($"Client's personal accounts {_client.FirstName} {_client.LastName}:");
            Console.WriteLine(presentationBankClientAccounts);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task DeleteClientAccountTest()
    {
        if (_client == null)
            return;

        var bankClientAccounts = await _clientService.GetClientAccountsAsync(_client.ClientId);

        var account = bankClientAccounts.LastOrDefault();

        if (account == null)
        {
            Console.WriteLine("Client account for deleting not found.");
            return;
        }

        try
        {
            Console.WriteLine($"Let's delete the account {account.AccountNumber}:");

            await _clientService.DeleteClientAccountAsync(account.AccountId);

            Console.WriteLine($"Client's personal accounts {_client.FirstName} {_client.LastName}:");

            var presentationBankClientAccounts =
                await _clientService.GetPresentationClientAccountsAsync(_client.ClientId);

            Console.WriteLine(presentationBankClientAccounts);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task UpdateClientTest()
    {
        if (_client == null)
            return;

        Console.WriteLine("Let's change the client's:");
        Console.WriteLine(
            $"Before the change {_client.FirstName} {_client.LastName}. id {_client.ClientId}");

        var newClient = _testDataGenerator.GenerateRandomBankClient();
        try
        {
            await _clientService.UpdateClientAsync(_client.ClientId, newClient);

            Console.WriteLine(
                $"After the change {_client.FirstName} {_client.LastName}. id {_client.ClientId}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task DeleteClientTest()
    {
        if (_client == null)
            return;

        Console.WriteLine($"Delete client by id - {_client.ClientId}");

        try
        {
            await _clientService.DeleteClientAsync(_client.ClientId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task GetClientsWithFilterTest()
    {
        const string clientFirstName = "Al";

        Console.WriteLine($"We will display clients with the name {clientFirstName}:");
        try
        {
            var filteredClients = await _clientService.ClientsWithFilterAndPaginationAsync(1, 100, clientFirstName);

            Console.WriteLine("Clients:");

            var mess = string.Join("\n",
                filteredClients.Select(client =>
                    $"First name {client.FirstName}, last name {client.LastName}, date of birth {client.DateOfBirth:D}"));

            Console.WriteLine(mess);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}