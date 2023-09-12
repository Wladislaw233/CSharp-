using BankingSystemServices.Models;

namespace Services.Storage;

public interface IClientStorage : IStorage<Client>
{
    void AddAccount(Client client, Account account);
    void UpdateAccount(Account account, Account newAccount);
    void DeleteAccount(Account account);
    Dictionary<Client, List<Account>> ClientWithAccountsList { get; }
}