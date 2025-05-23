namespace Tutorial11.Repositories;

public interface IDoctorRepository
{
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
}