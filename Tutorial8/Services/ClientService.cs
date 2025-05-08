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
            
            // Select a client with the specified id
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
            
            // Select all the information about a trip with information about registration date and payment date 
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

    public async Task<ClientDTO> PostClient(ClientDTO clientDto)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();       
            
            // Insert a new entry into client table and capture its id to later return in the controller
            var command = new SqlCommand("INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel) Values (@FirstName, @LastName, @Email, @Telephone, @Pesel) SELECT SCOPE_IDENTITY();", conn);
            
            command.Parameters.AddWithValue("@FirstName", clientDto.FirstName);
            command.Parameters.AddWithValue("@LastName", clientDto.LastName);
            command.Parameters.AddWithValue("@Email", clientDto.Email);
            command.Parameters.AddWithValue("@Telephone", clientDto.Telephone ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Pesel", clientDto.Pesel ?? (object)DBNull.Value);
            
            var result = await command.ExecuteScalarAsync();

            clientDto.Id = Convert.ToInt32(result);
        }

        return clientDto;
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