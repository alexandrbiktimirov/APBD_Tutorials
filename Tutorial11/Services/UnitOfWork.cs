using Microsoft.EntityFrameworkCore.Storage;
using Tutorial11.Data;
using Tutorial11.Repositories;

namespace Tutorial11.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _dbContext;
    private IDbContextTransaction? _transaction;

    public IPrescriptionRepository PrescriptionRepo { get; }
    public IPatientRepository PatientRepo { get; }
    public IMedicamentRepository MedicamentRepo { get; }
    public IDoctorRepository DoctorRepo { get; }

    public UnitOfWork(DatabaseContext dbContext,
        IPrescriptionRepository prescriptionRepository,
        IPatientRepository patientRepository,
        IMedicamentRepository medicamentRepository,
        IDoctorRepository doctorRepository)
    {
        _dbContext = dbContext;
        PrescriptionRepo = prescriptionRepository;
        PatientRepo = patientRepository;
        MedicamentRepo = medicamentRepository;
        DoctorRepo = doctorRepository;
    }
    
    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction != null) return;
        _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        if (_transaction == null) return;
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (_transaction == null) return;
        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }
}