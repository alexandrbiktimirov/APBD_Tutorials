using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface IClientsService
{
    public Task<bool> DoesClientExist(int id);
    public Task<List<ClientTripDTO>> GetClientTrips(int id);
    public Task<ClientDTO> PostClient(ClientDTO clientDto);
    public Task<string> PutClient(int id, int tripId);
    public Task<ClientDTO> DeleteClient(int id, int tripId);
}