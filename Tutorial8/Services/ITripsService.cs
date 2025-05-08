using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface ITripsService
{
    Task<List<TripDto>> GetTrips();
    Task<bool> DoesTripExist(int id);
    Task<bool> CheckCapacity(int id);
    Task<TripDto> GetTrip(int id);
}