using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Models.DTO;
using BankingSystemServices.Services;
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
        catch (InvalidOperationException exc)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(exc,
                "An error occurred while performing the operation.");
            _logger.Log(LogLevel.Error, exc, mess);
            return BadRequest(mess);
        }
        catch (ArgumentException exc)
        {
            var mess = ExceptionHandlingService.ArgumentExceptionHandler(exc,
                "An error occurred while adding the client to the database.");
            _logger.Log(LogLevel.Error, exc, mess);
            return BadRequest(mess);
        }
        catch (PropertyValidationException exc)
        {
            var mess = ExceptionHandlingService.PropertyValidationExceptionHandler(exc);
            _logger.Log(LogLevel.Error, exc, mess);
            return BadRequest(mess);
        }
        catch (Exception exc)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(exc);
            _logger.Log(LogLevel.Error, exc, mess);
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
        catch (ArgumentException exc)
        {
            var mess = ExceptionHandlingService.ArgumentExceptionHandler(exc,
                "An error occurred while updating the client in the database.");
            _logger.Log(LogLevel.Error, exc, mess);
            return BadRequest(mess);
        }
        catch (PropertyValidationException exc)
        {
            var mess = ExceptionHandlingService.PropertyValidationExceptionHandler(exc);
            _logger.Log(LogLevel.Error, exc, mess);
            return BadRequest(mess);
        }
        catch (Exception exc)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(exc);
            _logger.Log(LogLevel.Error, exc, mess);
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
        catch (ArgumentException exc)
        {
            var mess = ExceptionHandlingService.ArgumentExceptionHandler(exc,
                "An error occurred when deleting a client from the database.");
            _logger.Log(LogLevel.Error, exc, mess);
            return BadRequest(mess);
        }
        catch (Exception exc)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(exc);
            _logger.Log(LogLevel.Error, exc, mess);
            return BadRequest(mess);
        }
    }
}