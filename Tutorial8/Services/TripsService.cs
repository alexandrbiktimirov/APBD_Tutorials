using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public class TripsService(IConfiguration configuration) : ITripsService
{
    private readonly string? _connectionString = configuration.GetConnectionString("DefaultConnection");

    public async Task<List<TripDTO>> GetTrips()
    {
        var trips = new List<TripDTO>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            var tripCommand =
                new SqlCommand("SELECT IdTrip, Name, Description, DateFrom, DateTo, MaxPeople FROM Trip", conn);

            using (var tripReader = await tripCommand.ExecuteReaderAsync())
            {
                while (await tripReader.ReadAsync())
                {
                    trips.Add(new TripDTO
                    {
                        Id = tripReader.GetInt32(0),
                        Name = tripReader.GetString(1),
                        Description = tripReader.IsDBNull(2) ? null : tripReader.GetString(2),
                        DateFrom = tripReader.GetDateTime(3),
                        DateTo = tripReader.GetDateTime(4),
                        MaxPeople = tripReader.GetInt32(5),
                        Countries = new List<CountryDTO>(),
                    });
                }
            }

            var countryCommand = new SqlCommand("""
                                            
                                                            SELECT ct.IdTrip, c.Name
                                                            FROM Country_Trip ct
                                                            JOIN Country c ON c.IdCountry = ct.IdCountry
                                            """, conn);

            using (var countryReader = await countryCommand.ExecuteReaderAsync())
            {
                while (await countryReader.ReadAsync())
                {
                    int tripId = countryReader.GetInt32(0);
                    string countryName = countryReader.GetString(1);

                    var trip = trips.FirstOrDefault(t => t.Id == tripId);
                    if (trip != null)
                    {
                        trip.Countries.Add(new CountryDTO {Name = countryName});
                    }
                }
            }
        }

        return trips;
    }

    public async Task<bool> DoesTripExist(int id)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            var command = new SqlCommand("SELECT 1 FROM Trip WHERE IdTrip = @id", conn);
            command.Parameters.AddWithValue("@id", id);

            var result = await command.ExecuteReaderAsync();
            return result != null;
        }
    }

    public async Task<TripDTO> GetTrip(int id)
    {
        TripDTO? trip = null;

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            
            var tripCommand = new SqlCommand("SELECT * FROM Trip WHERE IdTrip = @id", conn);
            tripCommand.Parameters.AddWithValue("@id", id);
            
            using (var tripReader = await tripCommand.ExecuteReaderAsync())
            {
                if (await tripReader.ReadAsync())
                {
                    trip = new TripDTO
                    {
                        Id = tripReader.GetInt32(0),
                        Name = tripReader.GetString(1),
                        Description = tripReader.IsDBNull(2) ? null : tripReader.GetString(2),
                        DateFrom = tripReader.GetDateTime(3),
                        DateTo = tripReader.GetDateTime(4),
                        MaxPeople = tripReader.GetInt32(5),
                        Countries = new List<CountryDTO>(),
                    };
                }
            }

            var countryCommand = new SqlCommand("""
                                                
                                                                SELECT *
                                                                FROM Country
                                                                WHERE IdCountry = @id
                                                """, conn);
            countryCommand.Parameters.AddWithValue("@id", id);

            using (var countryReader = await countryCommand.ExecuteReaderAsync())
            {
                while (await countryReader.ReadAsync())
                {
                    trip.Countries.Add(new CountryDTO() {Name = countryReader.GetString(1) });
                }
            }
        }

        return trip;
    }
}