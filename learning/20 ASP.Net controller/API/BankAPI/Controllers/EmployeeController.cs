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
public class EmployeeController : ControllerBase
{
    private readonly EmployeeService _employeeService;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(BankingSystemDbContext bankingSystemDbContext, ILogger<EmployeeController> logger)
    {
        _employeeService = new EmployeeService(bankingSystemDbContext);
        _logger = logger;
    }

    [HttpGet("GetEmployeeById/{employeeId:guid}")]
    public async Task<ActionResult<Employee>> GetEmployeeById(Guid employeeId)
    {
        var employee = await _employeeService.GetEmployeeById(employeeId);

        if (employee == null)
            return NotFound();

        return employee;
    }

    [HttpDelete("DeletingEmployeeById/{employeeId:guid}")]
    public async Task<ActionResult> DeletingEmployeeById(Guid employeeId)
    {
        try
        {
            await _employeeService.DeleteEmployee(employeeId);
            return Ok();
        }
        catch (ArgumentException exc)
        {
            var mess = ExceptionHandlingService.ArgumentExceptionHandler(exc,
                "An error occurred when deleting a employee from the database.");
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

    [HttpPost("AddingEmployee")]
    public async Task<ActionResult<Employee>> AddingEmployee(EmployeeDto employeeDto)
    {
        try
        {
            var employee = await _employeeService.AddEmployee(employeeDto);
            return Ok(employee);
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
                "An error occurred while adding the employee to the database.");
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

    [HttpPut("UpdatingEmployee/{employeeId:guid}")]
    public async Task<ActionResult<Employee>> UpdatingEmployee(Guid employeeId, EmployeeDto employeeDto)
    {
        try
        {
            var employee = await _employeeService.UpdateEmployee(employeeId, employeeDto);
            return Ok(employee);
        }
        catch (ArgumentException exc)
        {
            var mess = ExceptionHandlingService.ArgumentExceptionHandler(exc,
                "An error occurred while updating the employee in the database.");
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
}