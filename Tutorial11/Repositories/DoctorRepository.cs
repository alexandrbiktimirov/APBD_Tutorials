using Microsoft.EntityFrameworkCore;
using Tutorial11.Data;

namespace Tutorial11.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly DatabaseContext _dbContext;

    public DoctorRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Doctors.AnyAsync(doc => doc.IdDoctor == id, cancellationToken);
    }
}