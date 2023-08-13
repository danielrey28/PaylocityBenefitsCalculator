using Api.Dtos.Paycheck;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Data.SqlClient;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PaycheckController : ControllerBase
    {
        private readonly IPaycheckService _paycheckService;

        public PaycheckController(IPaycheckService paycheckService)
        {
            _paycheckService = paycheckService;
        }
        [SwaggerOperation(Summary = "Get employee paycheck by id")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GetPaycheckDto>>> Get(int id)
        {
            try
            {
                var paycheck = await _paycheckService.GetPaycheck(id);

                if (paycheck == null)
                {
                    return NotFound();
                }

                return new ApiResponse<GetPaycheckDto>
                {
                    Data = paycheck,
                };
            }
            catch (SqlException ex)
            {
                return new ApiResponse<GetPaycheckDto>
                {
                    Data = null,
                    Error = ex.Message,
                    Message = "An error occurred retrieving paycheck",
                    Success = false
                };
            }
        }
    }
}
