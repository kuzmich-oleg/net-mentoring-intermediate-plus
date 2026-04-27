using Microsoft.EntityFrameworkCore;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Common.Extensions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Payments;

internal sealed class PaymentReadRepository : IPaymentReadRepository
{
    private readonly TicketingDbContext _dbContext;

    private IQueryable<PaymentEntity> ActivePayments
        => _dbContext.Payments.Where(x => !x.IsDeleted);

    public PaymentReadRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Payment?> GetByIdAsync(Guid paymentId, CancellationToken cancellationToken)
    {
        var paymentEntity = await ActivePayments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == paymentId, cancellationToken);

        var paymentModel = paymentEntity.MapIfNotNull(PaymentMapper.FromEntity);
        return paymentModel;
    }
}
