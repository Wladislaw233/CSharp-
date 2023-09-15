using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Models.DTO;
using BankingSystemServices.Services;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace BankAPI.Controllers;

/// <summary>
///     Controller to work with client service.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly ILogger<ClientController> _logger;

    public ClientController(IClientService clientService, ILogger<ClientController> logger)
    {
        _clientService = clientService;
        _logger = logger;
    }

    /// <summary>
    ///     Returns the found client by ID.
    /// </summary>
    /// <param name="clientId">Client guid.</param>
    /// <returns>
    ///     HTTP 200 OK and client if client is found.
    ///     HTTP 404 NotFound and error message if client is not found.
    ///     HTTP 500 InternalServerError and error massage if an unexpected error occurs on the server.
    /// </returns>
    [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpGet("GetClientById/{clientId:guid}")]
    public async Task<ActionResult<Client>> GetClientById(Guid clientId)
    {
        try
        {
            var client = await _clientService.GetClientByIdAsync(clientId);
            return Ok(client);
        }
        catch (ValueNotFoundException exc)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(exc);
            _logger.Log(LogLevel.Error, exc, mess);
            return NotFound(mess);
        }
        catch (Exception exc)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(exc);
            _logger.Log(LogLevel.Error, exc, mess);
            return StatusCode(StatusCodes.Status500InternalServerError, mess);
        }
    }

    /// <summary>
    ///     Adds a client with the passed data in the request.
    /// </summary>
    /// <param name="clientDto">employee data.</param>
    /// <returns>
    ///     HTTP 200 Ok and client if client is adding.
    ///     HTTP 400 BadRequest and error message if client is not adding.
    ///     HTTP 500 InternalServerError and error massage if an unexpected error occurs on the server.
    /// </returns>
    [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpPost("AddClient")]
    public async Task<ActionResult<Client>> AddClient(ClientDto clientDto)
    {
        try
        {
            var client = await _clientService.AddClientAsync(clientDto);
            return Ok(client);
        }
        catch (ArgumentException exc)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(exc);
            _logger.Log(LogLevel.Error, exc, mess);
            return BadRequest(mess);
        }
        catch (PropertyValidationException exc)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(exc);
            _logger.Log(LogLevel.Error, exc, mess);
            return BadRequest(mess);
        }
        catch (Exception exc)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(exc);
            _logger.Log(LogLevel.Error, exc, mess);
            return StatusCode(StatusCodes.Status500InternalServerError, mess);
        }
    }

    /// <summary>
    ///     Updates the data of the client found by ID with the data passed in the request.
    /// </summary>
    /// <param name="clientId">Client guid.</param>
    /// <param name="clientDto">Client data.</param>
    /// <returns>
    ///     HTTP 200 Ok and updated client if client is updated.
    ///     HTTP 400 BadRequest and error message if client is not updated.
    ///     HTTP 404 NotFound and error message if client is not found.
    ///     HTTP 500 InternalServerError and error massage if an unexpected error occurs on the server.
    /// </returns>
    [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateClient/{clientId:guid}")]
    public async Task<ActionResult<Client>> UpdateClient(Guid clientId, ClientDto clientDto)
    {
        try
        {
            var client = await _clientService.UpdateClientAsync(clientId, clientDto);
            return Ok(client);
        }
        catch (ValueNotFoundException exc)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(exc);
            _logger.Log(LogLevel.Error, exc, mess);
            return NotFound(mess);
        }
        catch (ArgumentException exc)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(exc);
            _logger.Log(LogLevel.Error, exc, mess);
            return BadRequest(mess);
        }
        catch (PropertyValidationException exc)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(exc);
            _logger.Log(LogLevel.Error, exc, mess);
            return BadRequest(mess);
        }
        catch (Exception exc)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(exc);
            _logger.Log(LogLevel.Error, exc, mess);
            return StatusCode(StatusCodes.Status500InternalServerError, mess);
        }
    }

    /// <summary>
    ///     Deletes the client by ID.
    /// </summary>
    /// <param name="clientId">client guid.</param>
    /// <returns>
    ///     HTTP 200 Ok if client is deleted.
    ///     HTTP 400 BadRequest and error message if client is not deleted.
    ///     HTTP 500 InternalServerError and error massage if an unexpected error occurs on the server.
    /// </returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpDelete("DeleteClientById/{clientId:guid}")]
    public async Task<ActionResult> DeleteClientById(Guid clientId)
    {
        try
        {
            await _clientService.DeleteClientAsync(clientId);
            return Ok();
        }
        catch (ValueNotFoundException exc)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(exc);
            _logger.Log(LogLevel.Error, exc, mess);
            return NotFound(mess);
        }
        catch (Exception exc)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(exc);
            _logger.Log(LogLevel.Error, exc, mess);
            return StatusCode(StatusCodes.Status500InternalServerError, mess);
        }
    }
}