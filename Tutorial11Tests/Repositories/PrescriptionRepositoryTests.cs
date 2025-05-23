using Tutorial11.Models;
using Tutorial11.Repositories;
using Tutorial11.Tests.Utils;

namespace Tutorial11.Tests.Repositories;

public class PrescriptionRepositoryTests
{
    [Fact]
    public async Task CreateNewAsync_AddsPrescriptionAndReturnsGeneratedId()
    {
        var context = TestUtils.CreateInMemoryDatabaseContext();
        var repo = new PrescriptionRepository(context);

        context.Doctors.Add(new Doctor { IdDoctor = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@gmail.com" });
        context.Patients.Add(new Patient { IdPatient = 1, FirstName = "Jane", LastName = "Doe", BirthDate = DateTime.Today });
        await context.SaveChangesAsync();

        var prescription = new Prescription
        {
            Date = DateTime.Today,
            DueDate = DateTime.Today.AddDays(1),
            IdDoctor = 1,
            IdPatient = 1
        };

        var id = await repo.CreateNewAsync(prescription, CancellationToken.None);

        Assert.True(id > 0);
        var saved = await context.Prescriptions.FindAsync(id);
        Assert.NotNull(saved);
        Assert.Equal(1, saved.IdDoctor);
        Assert.Equal(1, saved.IdPatient);
    }
    
    [Fact]
    public async Task AddManyMedicamentsToPrescriptionAsync_AddsRowsToDatabase()
    {
        var context = TestUtils.CreateInMemoryDatabaseContext();
        var repo = new PrescriptionRepository(context);

        var meds = new List<PrescriptionMedicament>
        {
            new() { IdPrescription = 1, IdMedicament = 1, Dose = 10, Details = "Details" },
            new() { IdPrescription = 1, IdMedicament = 2, Dose = 5, Details = "Details" }
        };

        await repo.AddManyMedicamentsToPrescriptionAsync(meds, CancellationToken.None);

        var count = context.PrescriptionMedicaments.Count();
        Assert.Equal(2, count);
    }
    
    [Fact]
    public async Task GetAllByPatientIdAsync_ReturnsPrescriptionsWithRelatedData()
    {
        var context = TestUtils.CreateInMemoryDatabaseContext();

        const int patientId = 1;

        context.Doctors.Add(new Doctor { IdDoctor = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@gmail.com" });

        context.Medicaments.Add(new Medicament { IdMedicament = 1, Name = "Medicament 1", Description = "Description", Type = "Type" });

        context.Prescriptions.Add(new Prescription
        {
            IdPrescription = 1,
            IdPatient = patientId,
            IdDoctor = 1,
            Date = DateTime.Today,
            DueDate = DateTime.Today.AddDays(5),
            PrescriptionMedicaments = new List<PrescriptionMedicament>
            {
                new() { IdMedicament = 1, Dose = 100, Details = "Once a day" }
            }
        });

        await context.SaveChangesAsync();

        var repo = new PrescriptionRepository(context);

        var result = await repo.GetAllByPatientIdAsync(patientId, CancellationToken.None);

        Assert.Single(result);
        var rx = result[0];
        Assert.Equal(1, rx.IdDoctor);
        Assert.NotNull(rx.Doctor);
        Assert.Single(rx.PrescriptionMedicaments);
        Assert.NotNull(rx.PrescriptionMedicaments.First().Medicament);
    }
}