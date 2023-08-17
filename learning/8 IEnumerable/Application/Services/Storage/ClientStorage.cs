using System.Collections;
using Models;

namespace Services.Storage;

public class ClientStorage : IEnumerable<Client>
{
    private readonly List<Client> _bankClients = new();

    public void AddBankClients(int numberOfClients = 10)
    {
        _bankClients.AddRange(TestDataGenerator.GenerateListWitchBankClients(numberOfClients));
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