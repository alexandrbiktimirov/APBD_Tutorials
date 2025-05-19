using Microsoft.Data.SqlClient;
using Tutorial11.DTOs;

namespace Tutorial11.Services;

public class DbService(IConfiguration configuration) : IDbService
{
    private readonly string? _connectionString = configuration.GetConnectionString("DefaultConnection");

    
    
    public Task<bool> DoesPatientExist()
    {
        
    }
    
    public async Task<PrescriptionDto> PostPrescription(PrescriptionDto prescription)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.OpenAsync();
            
            
        }
    }
}