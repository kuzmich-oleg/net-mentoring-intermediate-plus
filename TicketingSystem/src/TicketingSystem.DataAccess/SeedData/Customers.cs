using TicketingSystem.DataAccess.Entities;

namespace TicketingSystem.DataAccess.SeedData;

internal static class Customers
{
    internal static CustomerEntity[] DefaultCustomers = [
        new CustomerEntity
        {
            Id = CommonSeedData.DefaultCustomerId,
            UserId = CommonSeedData.DefaultUserId,
            FirstName = "Test",
            LastName = "User",
            CreatedAt = CommonSeedData.CreatedAt
        }
    ];
}
