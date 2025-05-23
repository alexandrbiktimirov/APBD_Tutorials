using Tutorial11.Models;
using Tutorial11.Tests.Utils;

namespace Tutorial11.Tests.Repositories;

public class MedicamentRepository
{
    [Fact]
    public async Task AllExists_ReturnsTrue_WhenAllExists()
    {
        var context = TestUtils.CreateInMemoryDatabaseContext();
        var medicament = new Medicament
        {
            IdMedicament = 1,
            Description = "Description",
            Name = "Medicament 1",
            Type = "Type"
        };
        var medicament2 = new Medicament
        {
            IdMedicament = 2,
            Description = "Description",
            Name = "Medicament 2",
            Type = "Type"
        };
        context.Medicaments.AddRange(medicament, medicament2);
        await context.SaveChangesAsync();

        var repo = new Tutorial11.Repositories.MedicamentRepository(context);

        var exists = await repo.AllExistAsync(new[] { 1, 2 }, CancellationToken.None);
        
        Assert.True(exists);
    }

    [Fact]
    public async Task AllExists_ReturnsFalse_WhenAnyNotFound()
    {
        var context = TestUtils.CreateInMemoryDatabaseContext();
        
        var repo = new Tutorial11.Repositories.MedicamentRepository(context);

        var exists = await repo.AllExistAsync(new[] { 1, 2 }, CancellationToken.None);
        
        Assert.False(exists);
    }
}