using Microsoft.EntityFrameworkCore;
using Tutorial11.Data;
using Tutorial11.Models;

namespace Tutorial11.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly DatabaseContext _dbContext;

    public PatientRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Patients.AnyAsync(p => p.IdPatient == id, cancellationToken);
    }

    public async Task<int> CreateNewAsync(Patient patient, CancellationToken cancellationToken)
    {
        await _dbContext.Patients.AddAsync(patient, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return patient.IdPatient;
    }

    public async Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Patients
            .FirstOrDefaultAsync(p => p.IdPatient == id, cancellationToken);
    }
}