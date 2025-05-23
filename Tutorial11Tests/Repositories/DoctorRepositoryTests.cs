using Tutorial11.Models;
using Tutorial11.Repositories;
using Tutorial11.Tests.Utils;

namespace Tutorial11.Tests.Repositories;

public class DoctorRepositoryTests
{
    [Fact]
    public async Task ExistsAsync_ReturnsTrue_WhenDoctorExists()
    {
        var context = TestUtils.CreateInMemoryDatabaseContext();
        context.Doctors.Add(new Doctor
        {
            IdDoctor = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@gmail.com",
        });
        await context.SaveChangesAsync();

        var repo = new DoctorRepository(context);

        var exists = await repo.ExistsAsync(1, CancellationToken.None);

        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsAsync_ReturnsFalse_WhenDoctorDoesNotExist()
    {
        var context = TestUtils.CreateInMemoryDatabaseContext();
        var repo = new DoctorRepository(context);

        var exists = await repo.ExistsAsync(1, CancellationToken.None);
        Assert.False(exists);
    }
}