using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial8.Models.DTOs;
using Tutorial8.Services;

namespace Tutorial8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientTrips(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid number provided");
            }
            
            if ( !await _clientService.DoesClientExist(id))
            {
                return NotFound();
            }

            var trips = await _clientService.GetClientTrips(id);

            return trips.Count == 0 ? Ok("Client doesn't have any trips") : Ok(trips);
        }
    }
}
