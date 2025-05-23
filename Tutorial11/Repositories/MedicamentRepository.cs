using Microsoft.EntityFrameworkCore;
using Tutorial11.Data;

namespace Tutorial11.Repositories;

public class MedicamentRepository : IMedicamentRepository
{
    private readonly DatabaseContext _dbContext;

    public MedicamentRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<bool> AllExistAsync(IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var distinctIds = ids.Distinct().ToList();
        var count = await _dbContext.Medicaments
            .Where(med => distinctIds.Contains(med.IdMedicament))
            .CountAsync(cancellationToken);
    
        return count == distinctIds.Count;
    }
}