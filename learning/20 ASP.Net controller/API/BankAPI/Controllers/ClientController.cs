using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace BankAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly ClientService _clientService;
    private readonly ILogger<ClientController> _logger;

    public ClientController(BankingSystemDbContext bankingSystemDbContext, ILogger<ClientController> logger)
    {
        _clientService = new ClientService(bankingSystemDbContext);
        _logger = logger;
    }

    [HttpGet("GetClientById/{clientId:guid}")]
    public async Task<ActionResult<Client>> GetClientById(Guid clientId)
    {
        var client = await _clientService.GetClientById(clientId);

        if (client == null)
            return NotFound();

        return client;
    }

    [HttpPost("AddingClient")]
    public async Task<ActionResult<Client>> AddingClient(ClientDto clientDto)
    {
        try
        {
            var client = await _clientService.AddClient(clientDto);
            return Ok(client);
        }
        catch (CustomException ex)
        {
            var mess = ExceptionHandlingService.CustomExceptionHandling(ex,
                "An error occurred while adding the client to the database: ");
            _logger.Log(LogLevel.Error, ex, mess);
            return BadRequest(mess);
        }
        catch (Exception ex)
        {
            var mess = ExceptionHandlingService.ExceptionHandling(ex);
            _logger.Log(LogLevel.Error, ex, mess);
            return BadRequest(mess);
        }
    }

    [HttpPut("UpdatingClient/{clientId:guid}")]
    public async Task<ActionResult<Client>> UpdatingClient(Guid clientId, ClientDto clientDto)
    {
        try
        {
            var client = await _clientService.UpdateClient(clientId, clientDto);
            return Ok(client);
        }
        catch (CustomException ex)
        {
            var mess = ExceptionHandlingService.CustomExceptionHandling(ex,
                "An error occurred while updating the client in the database: ");
            _logger.Log(LogLevel.Error, ex, mess);
            return BadRequest(mess);
        }
        catch (Exception ex)
        {
            var mess = ExceptionHandlingService.ExceptionHandling(ex);
            _logger.Log(LogLevel.Error, ex, mess);
            return BadRequest(mess);
        }
    }

    [HttpDelete("DeletingClientById/{clientId:guid}")]
    public async Task<ActionResult> DeletingClientById(Guid clientId)
    {
        try
        {
            await _clientService.DeleteClient(clientId);
            return Ok();
        }
        catch (CustomException ex)
        {
            var mess = ExceptionHandlingService.CustomExceptionHandling(ex,
                "An error occurred when deleting a client from the database: ");
            _logger.Log(LogLevel.Error, ex, mess);
            return BadRequest(mess);
        }
        catch (Exception ex)
        {
            var mess = ExceptionHandlingService.ExceptionHandling(ex);
            _logger.Log(LogLevel.Error, ex, mess);
            return BadRequest(mess);
        }
    }
}