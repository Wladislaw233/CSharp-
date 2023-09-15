using BankingSystemServices.Models;
using BankingSystemServices.Models.DTO;

namespace Services.Interfaces;

public interface IClientService
{
    public Task<Client> AddClientAsync(ClientDto clientDto);

    public Task<Client> UpdateClientAsync(Guid clientId, ClientDto newClientDto);

    public Task DeleteClientAsync(Guid client);

    public Task<Client> GetClientByIdAsync(Guid clientId);
}