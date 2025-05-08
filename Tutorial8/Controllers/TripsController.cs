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
        
        /// <summary>
        /// This endpoint returns a list of all trips in the database.
        /// </summary>
        /// <returns>200 OK.</returns>
        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            var trips = await _tripsService.GetTrips();
            return Ok(trips);
        }
        
        /// <summary>
        /// This endpoint returns a trip with the specified id.
        /// </summary>
        /// <returns>200 OK or 404 if not found.</returns>
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
