using Tutorial11.Repositories;

namespace Tutorial11.Services;

public interface IUnitOfWork
{
    IPrescriptionRepository PrescriptionRepo { get; }
    IPatientRepository PatientRepo { get; }
    IMedicamentRepository MedicamentRepo { get; }
    IDoctorRepository DoctorRepo { get; }
    
    
    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitAsync(CancellationToken cancellationToken);
    Task RollbackAsync(CancellationToken cancellationToken);
}