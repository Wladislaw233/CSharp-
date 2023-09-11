using BankingSystemServices.Models;
using BankingSystemServices.Services;
using BankingSystemServices.Exceptions;
using Services.Storage;

namespace Services;

public class ClientService
{
    private readonly ClientStorage _clientStorage;

    public ClientService(ClientStorage clientStorage)
    {
        _clientStorage = clientStorage;
    }
    public List<Client> GetClientsByFilters(string firstNameFilter = "",
        string lastNameFilter = "", string phoneNumberFilter = "", DateTime? minDateOfBirth = null,
        DateTime? maxDateOfBirth = null)
    {
        IEnumerable<Client> filteredClients = _clientStorage;
        if (!string.IsNullOrWhiteSpace(firstNameFilter))
            filteredClients = filteredClients.Where(client => client.FirstName == firstNameFilter);
        if (!string.IsNullOrWhiteSpace(lastNameFilter))
            filteredClients = filteredClients.Where(client => client.LastName == lastNameFilter);
        if (!string.IsNullOrWhiteSpace(phoneNumberFilter))
            filteredClients = filteredClients.Where(client => client.PhoneNumber == phoneNumberFilter);
        if (minDateOfBirth.HasValue)
            filteredClients = filteredClients.Where(client => client.DateOfBirth >= minDateOfBirth);
        if (maxDateOfBirth.HasValue)
            filteredClients = filteredClients.Where(client => client.DateOfBirth <= maxDateOfBirth);
        
        return filteredClients.ToList();
    }
}