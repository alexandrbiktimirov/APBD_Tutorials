namespace Tutorial11.Repositories;

public interface IMedicamentRepository
{
    Task<bool> AllExistAsync(IEnumerable<int> ids, CancellationToken cancellationToken);
}