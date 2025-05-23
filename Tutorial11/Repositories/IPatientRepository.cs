using Tutorial11.Models;

namespace Tutorial11.Repositories;

public interface IPatientRepository
{
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
    Task<int> CreateNewAsync(Patient patient, CancellationToken cancellationToken);
    Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken);
}