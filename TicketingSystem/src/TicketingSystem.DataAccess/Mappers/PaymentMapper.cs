using Riok.Mapperly.Abstractions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Mappers;

[Mapper]
internal static partial class PaymentMapper
{
    [MapperIgnoreSource(nameof(PaymentEntity.IsDeleted))]
    public static partial Payment FromEntity(PaymentEntity paymentEntity);

    [MapperIgnoreTarget(nameof(PaymentEntity.IsDeleted))]
    public static partial PaymentEntity ToEntity(Payment paymentModel);
}
