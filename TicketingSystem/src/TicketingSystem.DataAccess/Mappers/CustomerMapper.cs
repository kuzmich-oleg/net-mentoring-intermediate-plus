using Riok.Mapperly.Abstractions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Mappers;

[Mapper]
internal static partial class CustomerMapper
{
    [MapperIgnoreSource(nameof(CustomerEntity.IsDeleted))]
    public static partial Customer FromEntity(CustomerEntity customerEntity);

    [MapperIgnoreTarget(nameof(CustomerEntity.IsDeleted))]
    public static partial CustomerEntity ToEntity(Customer customerModel);
}
