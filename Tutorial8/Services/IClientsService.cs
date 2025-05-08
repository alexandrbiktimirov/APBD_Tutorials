using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface IClientsService
{
    public Task<bool> DoesClientExist(int id);
    public Task<List<ClientTripDto>> GetClientTrips(int id);
    public Task<ClientDto> PostClient(ClientDto clientDto);
    public Task<string> PutClient(int id, int tripId);
    public Task<int> DeleteClient(int id, int tripId);
}