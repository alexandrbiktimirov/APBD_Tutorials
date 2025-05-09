using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial9.Model.DTOs;
using Tutorial9.Services;

namespace Tutorial9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        public readonly IDbService _dbService;

        public WarehouseController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPut]
        public async Task<IActionResult> PutProduct(WarehouseDto dto)
        {
            if (dto.IdProduct <= 0 || !await _dbService.DoesProductExist(dto.IdProduct))
            {
                return BadRequest("Product does not exist");
            }

            if (!await _dbService.OrderExists(dto))
            {
                return BadRequest("Order does not exist");
            }
            
            return Ok();
        }
    }
}
