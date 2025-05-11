using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Infrastructure;
using Tutorial9.Exceptions;
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
            int id;

            try
            {
                id = await _dbService.PutProduct(dto);
            }
            catch (ProductDoesNotExistException e)
            {
                return BadRequest(e.Message);
            }
            catch (OrderDoesNotExistException e)
            {
                return BadRequest(e.Message);
            }
            catch (OrderHasBeenCompletedException e)
            {
                return BadRequest(e.Message);
            }
            catch (AmountIsNotGreaterThanZeroException e)
            {
                return BadRequest(e.Message);
            }
            catch (WarehouseDoesNotExistException e)
            {
                return BadRequest(e.Message);
            }
            catch (TransactionException e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
            
            return Ok(id);
        }
        
        [HttpPut("procedure")]
        public async Task<IActionResult> PutProductProcedure(WarehouseDto dto)
        {
            int result;

            try
            {
                result = await _dbService.ProcedureAsync(dto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
            return Ok(result);
        }
    }
}
