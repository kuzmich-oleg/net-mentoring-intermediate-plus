using Riok.Mapperly.Abstractions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Mappers;

[Mapper]
internal static partial class UserMapper
{
    [MapperIgnoreSource(nameof(UserEntity.IsDeleted))]
    public static partial User FromEntity(UserEntity userEntity);

    [MapperIgnoreTarget(nameof(UserEntity.IsDeleted))]
    public static partial UserEntity ToEntity(User userModel);
}
