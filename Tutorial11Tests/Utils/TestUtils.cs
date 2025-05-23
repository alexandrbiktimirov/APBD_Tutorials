using Microsoft.EntityFrameworkCore;
using Tutorial11.Data;

namespace Tutorial11.Tests.Utils;

public static class TestUtils
{
    public static DatabaseContext CreateInMemoryDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new DatabaseContext(options);

        return context;
    }
}