using System.Data.SqlClient;
using Api.Dtos.Dependent;
using Api.Models;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly IDependentsRepository _dependentsRepository;

    public DependentsController(IDependentsRepository dependentsRepository)
    {
        _dependentsRepository = dependentsRepository;
    }


    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        try
        {
            var dependent = await _dependentsRepository.GetAsync(id);

            if (dependent == null)
            {
                return NotFound(id);
            }

            return new ApiResponse<GetDependentDto>
            {
                Data = dependent
            };
        }
        catch (SqlException ex)
        {
            return new ApiResponse<GetDependentDto>
            {
                Data = null,
                Error = ex.Message,
                Message = "An error occurred retrieving dependents",
                Success = false
            };
        }
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        try
        {
            var dependents = await _dependentsRepository.GetAllAsync();

            return new ApiResponse<List<GetDependentDto>>
            {
                Data = dependents.ToList(),
            };
        }
        catch (SqlException ex)
        {
            return new ApiResponse<List<GetDependentDto>>
            {
                Data = null,
                Error = ex.Message,
                Message = "An error occurred retrieving dependents",
                Success = false
            };
        }
    }
}
