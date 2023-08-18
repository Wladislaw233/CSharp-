using Models;

namespace Services.Storage;

public interface IClientStorage : IStorage<Client>
{
    void AddAccount(Client client, string currencyCode = "USD", double amount = 0);
    void UpdateAccount(Client client, string accountNumber, string currencyCode = "", double amount = 0);
    void DeleteAccount(Client client, string accountNumber);
    Dictionary<Client, List<Account>> Data { get; }
}