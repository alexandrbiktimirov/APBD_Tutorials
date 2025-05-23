using Tutorial11.Models;

namespace Tutorial11.Repositories;

public interface IPrescriptionRepository
{
    Task<int> CreateNewAsync(Prescription prescription, CancellationToken cancellationToken);
    Task AddManyMedicamentsToPrescriptionAsync(List<PrescriptionMedicament> prescriptionMedicaments, CancellationToken cancellationToken);
    Task<List<Prescription>> GetAllByPatientIdAsync(int id, CancellationToken cancellationToken);
}