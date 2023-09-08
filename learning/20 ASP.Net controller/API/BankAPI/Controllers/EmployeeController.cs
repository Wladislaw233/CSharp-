using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Models.DTO;
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
        catch (CustomException ex)
        {
            var mess = ExceptionHandlingService.CustomExceptionHandling(ex,
                "An error occurred when deleting a employee from the database: ");
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

    [HttpPost("AddingEmployee")]
    public async Task<ActionResult<Employee>> AddingEmployee(EmployeeDto employeeDto)
    {
        try
        {
            var employee = await _employeeService.AddEmployee(employeeDto);
            return Ok(employee);
        }
        catch (CustomException ex)
        {
            var mess = ExceptionHandlingService.CustomExceptionHandling(ex,
                "An error occurred when adding a employee to the database: ");
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

    [HttpPut("UpdatingEmployee/{employeeId:guid}")]
    public async Task<ActionResult<Employee>> UpdatingEmployee(Guid employeeId, EmployeeDto employeeDto)
    {
        try
        {
            var employee = await _employeeService.UpdateEmployee(employeeId, employeeDto);
            return Ok(employee);
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
}