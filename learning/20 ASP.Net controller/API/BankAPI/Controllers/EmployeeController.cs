using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Models.DTO;
using BankingSystemServices.Services;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace BankAPI.Controllers;

/// <summary>
///     Controller to work with employee service.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }

    /// <summary>
    ///     Returns the found employee by ID.
    /// </summary>
    /// <param name="employeeId">Employee guid.</param>
    /// <returns>
    ///     HTTP 200 OK and employee if employee is found.
    ///     HTTP 404 NotFound and error message if employee is not found.
    ///     HTTP 500 InternalServerError and error massage if an unexpected error occurs on the server.
    /// </returns>
    [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpGet("GetEmployeeById/{employeeId:guid}")]
    public async Task<ActionResult<Employee>> GetEmployeeById(Guid employeeId)
    {
        try
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
            return Ok(employee);
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
    ///     deletes the employee by ID.
    /// </summary>
    /// <param name="employeeId">Employee guid.</param>
    /// <returns>
    ///     HTTP 200 Ok if employee is deleted.
    ///     HTTP 400 BadRequest and error message if employee is not deleted.
    ///     HTTP 500 InternalServerError and error massage if an unexpected error occurs on the server.
    /// </returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpDelete("DeleteEmployeeById/{employeeId:guid}")]
    public async Task<ActionResult> DeleteEmployeeById(Guid employeeId)
    {
        try
        {
            await _employeeService.DeleteEmployeeAsync(employeeId);
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

    /// <summary>
    ///     Adds a employee with the passed data in the request.
    /// </summary>
    /// <param name="employeeDto">employee data.</param>
    /// <returns>
    ///     HTTP 200 Ok and employee if employee is adding.
    ///     HTTP 400 BadRequest and error message if employee is not adding.
    ///     HTTP 500 InternalServerError and error massage if an unexpected error occurs on the server.
    /// </returns>
    [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpPost("AddEmployee")]
    public async Task<ActionResult<Employee>> AddEmployee(EmployeeDto employeeDto)
    {
        try
        {
            var employee = await _employeeService.AddEmployeeAsync(employeeDto);
            return Ok(employee);
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
    ///     updates the data of the employee found by ID with the data passed in the request.
    /// </summary>
    /// <param name="employeeId">Employee guid.</param>
    /// <param name="employeeDto">Employee data.</param>
    /// <returns>
    ///     HTTP 200 Ok and updated employee if employee is updated.
    ///     HTTP 400 BadRequest and error message if employee is not updated.
    ///     HTTP 404 NotFound and error message if employee is not found.
    ///     HTTP 500 InternalServerError and error massage if an unexpected error occurs on the server.
    /// </returns>
    [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateEmployee/{employeeId:guid}")]
    public async Task<ActionResult<Employee>> UpdateEmployee(Guid employeeId, EmployeeDto employeeDto)
    {
        try
        {
            var employee = await _employeeService.UpdateEmployeeAsync(employeeId, employeeDto);
            return Ok(employee);
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
            return BadRequest(mess);
        }
    }
}