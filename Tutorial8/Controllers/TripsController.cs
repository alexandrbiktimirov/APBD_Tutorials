using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial8.Services;

namespace Tutorial8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITripsService _tripsService;
        
        public TripsController(ITripsService tripsService)
        {
            _tripsService = tripsService;
        }
        
        // This endpoint returns the list of all trips in the database
        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            var trips = await _tripsService.GetTrips();
            return Ok(trips);
        }
        
        // This endpoint returns a specific trip with an id that user has written
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrip(int id)
        {
            if( !await _tripsService.DoesTripExist(id)){
                return NotFound();
            }
            
            var trip = await _tripsService.GetTrip(id);
            return Ok(trip);
        }
    }
}
