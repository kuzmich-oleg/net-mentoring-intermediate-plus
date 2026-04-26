using TicketingSystem.DataAccess.Entities;

namespace TicketingSystem.DataAccess.SeedData;

internal static class Users
{
    internal static UserEntity[] DefaultUsers = [
        new UserEntity
        {
            Id = CommonSeedData.DefaultUserId,
            Type = Domain.UserType.Customer,
            Email = CommonSeedData.DefaultUserEmail,
            CreatedAt = CommonSeedData.CreatedAt
        }
    ];
}
