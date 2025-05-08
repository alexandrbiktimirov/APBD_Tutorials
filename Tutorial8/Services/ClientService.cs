using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public class ClientService(IConfiguration configuration) : IClientService
{
    private readonly string? _connectionString = configuration.GetConnectionString("DefaultConnection");

    public async Task<bool> DoesClientExist(int id)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            var clientCommand = new SqlCommand("SELECT 1 FROM Client WHERE IdClient = @id", conn);
            clientCommand.Parameters.AddWithValue("@id", id);

            var result = clientCommand.ExecuteReaderAsync();
            return result != null;
        }
    }
    
    public async Task<List<ClientTripDTO>> GetClientTrips(int id)
    {
        List<ClientTripDTO> clientTrips = [];
        
        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            var query = """
                        
                                        SELECT t.IdTrip, t.Name, t.Description, t.DateFrom, t.DateTo, t.MaxPeople, ct.RegisteredAt, ct.PaymentDate 
                                        FROM Client_Trip ct
                                        JOIN Trip t ON t.IdTrip = ct.IdTrip
                                        WHERE ct.IdClient = @id
                        """;

            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@id", id);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    clientTrips.Add(new ClientTripDTO
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                        DateFrom = reader.GetDateTime(3),
                        DateTo = reader.GetDateTime(4),
                        MaxPeople = reader.GetInt32(5),
                        PaymentDate = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                        RegisteredAt = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    });
                }
            }
        }

        return clientTrips;
    }

    public Task<ClientDTO> PostClients()
    {
        throw new NotImplementedException();
    }

    public Task<ClientDTO> PutClient(int id, int tripId)
    {
        throw new NotImplementedException();
    }

    public Task<ClientDTO> DeleteClient(int id, int tripId)
    {
        throw new NotImplementedException();
    }
}