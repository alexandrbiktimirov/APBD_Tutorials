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
        private IClientsService _clientsService;
        private ITripsService _tripsService;

        public ClientsController(IClientsService clientsService, ITripsService tripsService)
        {
            _clientsService = clientsService;
            _tripsService = tripsService;
        }
        
        // This endpoint returns a list of trips that client had along with information about registration and payment dates
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientTrips(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid number provided");
            }
            
            if ( !await _clientsService.DoesClientExist(id))
            {
                return NotFound("Client does not exist");
            }

            var trips = await _clientsService.GetClientTrips(id);

            return trips.Count == 0 ? Ok("Client doesn't have any trips") : Ok(trips);
        }
        
        // This endpoint inserts a new client entry into database
        [HttpPost]
        public async Task<IActionResult> PostClient(ClientDTO clientDto)
        {
            var result = await _clientsService.PostClient(clientDto);

            return Created($"/api/clients/{result.Id}", result);
        }

        [HttpPut("{id}/trips/{tripId}")]
        public async Task<IActionResult> PutClient(int id, int tripId)
        {
            if (id <= 0 || tripId <= 0)
            {
                return BadRequest("Invalid numbers provided");
            }
            
            if ( !await _clientsService.DoesClientExist(id) || !await _tripsService.DoesTripExist(tripId))
            {
                return NotFound("Client or trip does not exist");
            }

            if (await _tripsService.CheckCapacity(tripId))
            {
                return BadRequest("Trip has the maximum number of people");
            }

            var message = await _clientsService.PutClient(id, tripId);
            return Ok(message);
        }
    }
}
