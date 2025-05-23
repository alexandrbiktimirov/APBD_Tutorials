using Tutorial11.DTOs;
using Tutorial11.Exceptions;
using Tutorial11.Models;
using Tutorial11.Models.DTOs;

namespace Tutorial11.Services;

public class PrescriptionService(IUnitOfWork uow) : IPrescriptionService
{
    public async Task<int> CreateNewPrescriptionAsync(CreatePrescriptionDto createPrescriptionDto,
        CancellationToken cancellationToken)
    {
        await uow.BeginTransactionAsync(cancellationToken);
        try
        {
            if (createPrescriptionDto.Medicaments.Count > 10)
                throw new TooMuchMedicationsException("One prescription can store only up to 10 medications.");

            if (createPrescriptionDto.DueDate <= createPrescriptionDto.Date)
                throw new InvalidPrescriptionDateException(
                    "Date of starting of prescription must be before its ending date.");

            var medicamentIds = createPrescriptionDto.Medicaments.Select(medicament => medicament.IdMedicament);
            if (!await uow.MedicamentRepo.AllExistAsync(medicamentIds, cancellationToken))
                throw new MedicamentDoesNotExistException("One or more medicaments were not found.");

            if (!await uow.DoctorRepo.ExistsAsync(createPrescriptionDto.Doctor.IdDoctor, cancellationToken))
                throw new DoctorDoesNotExistException("Doctor with provided id doesn't exist.");

            int patientId;

            if (!await uow.PatientRepo.ExistsAsync(createPrescriptionDto.Patient.IdPatient, cancellationToken))
            {
                var newPatient = new Patient
                {
                    FirstName = createPrescriptionDto.Patient.FirstName,
                    LastName = createPrescriptionDto.Patient.LastName,
                    BirthDate = createPrescriptionDto.Patient.BirthDate,
                };
                patientId = await uow.PatientRepo.CreateNewAsync(newPatient, cancellationToken);
            }
            else
            {
                patientId = createPrescriptionDto.Patient.IdPatient;
            }

            var newPrescription = new Prescription
            {
                Date = createPrescriptionDto.Date,
                DueDate = createPrescriptionDto.DueDate,
                IdPatient = patientId,
                IdDoctor = createPrescriptionDto.Doctor.IdDoctor
            };

            var prescriptionId = await uow.PrescriptionRepo.CreateNewAsync(newPrescription, cancellationToken);

            var prescriptionMedicaments = createPrescriptionDto.Medicaments.Select(m => new PrescriptionMedicament
            {
                IdPrescription = prescriptionId,
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Details = m.Description
            }).ToList();

            await uow.PrescriptionRepo.AddManyMedicamentsToPrescriptionAsync(prescriptionMedicaments, cancellationToken);

            await uow.CommitAsync(cancellationToken);
            return prescriptionId;
        }
        catch
        {
            await uow.RollbackAsync(cancellationToken);
            throw;
        }
    }
}