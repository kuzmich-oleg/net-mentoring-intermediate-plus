using Microsoft.EntityFrameworkCore;

namespace TicketingSystem.DataAccess.UnitTests;

internal class TicketingDbContextFactory
{
    public static TicketingDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<TicketingDbContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

        var context = new TicketingDbContext(options);

        return context;
    }
}
