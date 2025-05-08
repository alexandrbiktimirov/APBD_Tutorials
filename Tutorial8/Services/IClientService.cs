using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface IClientService
{
    public Task<bool> DoesClientExist(int id);
    public Task<List<ClientTripDTO>> GetClientTrips(int id);
    public Task<ClientDTO> PostClients();
    public Task<ClientDTO> PutClient(int id, int tripId);
    public Task<ClientDTO> DeleteClient(int id, int tripId);
}