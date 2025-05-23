using Tutorial11.Models;
using Tutorial11.Repositories;
using Tutorial11.Tests.Utils;

namespace Tutorial11.Tests.Repositories;

public class PatientRepositoryTests
{
    [Fact]
    public async Task ExistsAsync_ReturnsTrue_WhenDoctorExists()
    {
        var context = TestUtils.CreateInMemoryDatabaseContext();
        context.Patients.Add(new Patient
        {
            IdPatient = 1,
            FirstName = "John",
            LastName = "Doe",
            BirthDate = DateTime.Today,
        });
        await context.SaveChangesAsync();

        var repo = new PatientRepository(context);

        var exists = await repo.ExistsAsync(1, CancellationToken.None);

        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsAsync_ReturnsFalse_WhenDoctorDoesNotExist()
    {
        var context = TestUtils.CreateInMemoryDatabaseContext();
        var repo = new PatientRepository(context);

        var exists = await repo.ExistsAsync(1, CancellationToken.None);
        Assert.False(exists);
    }

    [Fact]
    public async Task CreateNewAsync_AddPatientAndReturnGeneratedId()
    {
        var context = TestUtils.CreateInMemoryDatabaseContext();
        var repo = new PatientRepository(context);

        var patient = new Patient
        {
            FirstName = "Jane",
            LastName = "Doe",
            BirthDate = DateTime.Today,
        };

        var id = await repo.CreateNewAsync(patient, CancellationToken.None);
        
        Assert.True(id > 0);
        var saved = await context.Patients.FindAsync(id);
        Assert.NotNull(saved);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsPatient()
    {
        var context = TestUtils.CreateInMemoryDatabaseContext();
        var repo = new PatientRepository(context);

        context.Patients.Add(new Patient
        {
            IdPatient = 1,
            FirstName = "John",
            LastName = "Doe",
            BirthDate = DateTime.Today,
        });
        await context.SaveChangesAsync();

        var patient = await repo.GetByIdAsync(1, CancellationToken.None);
        Assert.NotNull(patient);
    }
    
    [Fact]
    public async Task GetByIdAsync_ReturnsNothing_WhenInvalidId()
    {
        var context = TestUtils.CreateInMemoryDatabaseContext();
        var repo = new PatientRepository(context);

        var patient = await repo.GetByIdAsync(1, CancellationToken.None);
        Assert.Null(patient);
    }
}