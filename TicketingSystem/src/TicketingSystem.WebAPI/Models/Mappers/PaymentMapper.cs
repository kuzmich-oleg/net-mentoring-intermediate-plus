using Riok.Mapperly.Abstractions;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.WebAPI.Models.Mappers;

[Mapper]
public static partial class PaymentMapper
{
    [MapProperty(nameof(Payment.Status), nameof(PaymentResponse.Status), Use = nameof(MapStatus))]
    public static partial PaymentResponse ToResponse(Payment paymentModel);

    private static string MapStatus(PaymentStatus status)
        => status.ToString();
}
