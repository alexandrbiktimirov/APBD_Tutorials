﻿using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public class TripsService(IConfiguration configuration) : ITripsService
{
    private readonly string? _connectionString = configuration.GetConnectionString("DefaultConnection");

    public async Task<List<TripDto>> GetTrips()
    {
        var trips = new List<TripDto>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            
            // Select all the information from Trip table
            var tripCommand =
                new SqlCommand("SELECT * FROM Trip", conn);

            using (var tripReader = await tripCommand.ExecuteReaderAsync())
            {
                while (await tripReader.ReadAsync())
                {
                    trips.Add(new TripDto
                    {
                        Id = tripReader.GetInt32(0),
                        Name = tripReader.GetString(1),
                        Description = tripReader.IsDBNull(2) ? null : tripReader.GetString(2),
                        DateFrom = tripReader.GetDateTime(3),
                        DateTo = tripReader.GetDateTime(4),
                        MaxPeople = tripReader.GetInt32(5),
                        Countries = new List<CountryDto>(),
                    });
                }
            }
        
            // Select countries for each trip
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
                        trip.Countries.Add(new CountryDto {Name = countryName});
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
            
            // Select a trip with the specified id
            var command = new SqlCommand("SELECT 1 FROM Trip WHERE IdTrip = @id", conn);
            command.Parameters.AddWithValue("@id", id);

            var result = await command.ExecuteScalarAsync();
            return result != null;
        }
    }

    public async Task<TripDto> GetTrip(int id)
    {
        TripDto? trip = null;

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            
            // Select all the information about the trip with the specified id
            var tripCommand = new SqlCommand("SELECT * FROM Trip WHERE IdTrip = @id", conn);
            tripCommand.Parameters.AddWithValue("@id", id);
            
            using (var tripReader = await tripCommand.ExecuteReaderAsync())
            {
                if (await tripReader.ReadAsync())
                {
                    trip = new TripDto
                    {
                        Id = tripReader.GetInt32(0),
                        Name = tripReader.GetString(1),
                        Description = tripReader.IsDBNull(2) ? null : tripReader.GetString(2),
                        DateFrom = tripReader.GetDateTime(3),
                        DateTo = tripReader.GetDateTime(4),
                        MaxPeople = tripReader.GetInt32(5),
                        Countries = new List<CountryDto>(),
                    };
                }
            }
            
            // Select names of countries for each trip
            var countryCommand = new SqlCommand("""
                                                                SELECT c.Name
                                                                FROM Country_Trip ct
                                                                JOIN Country c ON c.IdCountry = ct.IdCountry
                                                                WHERE ct.IdTrip = @id
                                                
                                                """, conn);
            countryCommand.Parameters.AddWithValue("@id", id);

            using (var countryReader = await countryCommand.ExecuteReaderAsync())
            {
                while (await countryReader.ReadAsync())
                {
                    trip.Countries.Add(new CountryDto() {Name = countryReader.GetString(0) });
                }
            }
        }

        return trip;
    }

    public async Task<bool> CheckCapacity(int id)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            
            // Select maximum number of people for this trip
            var maxPeopleCommand= new SqlCommand("SELECT MaxPeople FROM Trip WHERE IdTrip = @id", conn);
            maxPeopleCommand.Parameters.AddWithValue("@id", id);
            var maxPeople = Convert.ToInt32(await maxPeopleCommand.ExecuteScalarAsync());

            // Select actual number of people currently on this trip 
            var actualPeopleCommand = new SqlCommand("SELECT COUNT(IdClient) FROM Client_Trip WHERE IdTrip = @id", conn);
            actualPeopleCommand.Parameters.AddWithValue("@id", id);
            var actualPeople = Convert.ToInt32(await actualPeopleCommand.ExecuteScalarAsync());

            return maxPeople == actualPeople;
        }
    }
}