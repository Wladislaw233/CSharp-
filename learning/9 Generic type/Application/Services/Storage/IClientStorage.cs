using BankingSystemServices;

namespace Services.Storage;

public interface IClientStorage : IStorage<Client>
{
    void Update(Client client, string? firstName = null, string? lastName = null, int? age = null,
        DateTime? dateOfBirth = null, string? phoneNumber = null, string? address = null, string? email = null,
        decimal? bonus = null);

    void AddAccount(Client client, string currencyCode = "USD", decimal amount = 0);
    void UpdateAccount(Client client, string accountNumber, string? currencyCode = null, decimal? amount = null);
    void DeleteAccount(Client client, string accountNumber);
    Dictionary<Client, List<Account>> Data { get; }
}