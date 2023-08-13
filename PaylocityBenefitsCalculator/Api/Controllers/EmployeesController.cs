using System.Data.SqlClient;
using Api.Dtos.Employee;
using Api.Models;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeesRepository _employeeRepository;

    public EmployeesController(IEmployeesRepository employeeRepository)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        try
        {
            var employee = await _employeeRepository.GetAsync(id);

            if (employee == null)
            {
                return NotFound(id);
            }

            //Returning a new ApiResponse instead of allocating a new variable. 
            //Success is true by default so it's unnecessary to assign true again.
            return new ApiResponse<GetEmployeeDto>
            {
                Data = employee,
            };
        }
        catch (SqlException ex)
        {
            return new ApiResponse<GetEmployeeDto>
            {
                Data = null,
                Error = ex.Message,
                Message = "An error occurred retrieving employees",
                Success = false
            };
        }
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        try
        {
            //Employees and their dependents are now retrieved from a sql database through the Employee Repository
            var employees = await _employeeRepository.GetAllAsync();

            return new ApiResponse<List<GetEmployeeDto>>
            {
                Data = employees.ToList(),
            };
        }
        catch (SqlException ex)
        {
            return new ApiResponse<List<GetEmployeeDto>>
            {
                Data = null,
                Error = ex.Message,
                Message = "An error occurred retrieving employees",
                Success = false
            };
        }
    }
}
