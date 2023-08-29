using System.Collections;
using BankingSystemServices;
using BankingSystemServices.Services;

namespace Services.Storage;

public class ClientStorage : IEnumerable<Client>
{
    private readonly List<Client> _bankClients = new();

    public void AddBankClients(Client client)
    {
        _bankClients.Add(client);
    }

    public IEnumerator<Client> GetEnumerator()
    {
        return _bankClients.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}