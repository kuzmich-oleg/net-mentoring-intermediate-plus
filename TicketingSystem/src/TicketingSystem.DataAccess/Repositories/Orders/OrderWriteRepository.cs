using Microsoft.EntityFrameworkCore;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Orders;

internal sealed class OrderWriteRepository : IOrderWriteRepository
{
    private readonly TicketingDbContext _dbContext;

    public OrderWriteRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(Order orderModel, CancellationToken cancellationToken)
    {
        var orderEntity = OrderMapper.ToEntity(orderModel);

        _dbContext.Orders.Add(orderEntity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return orderEntity.Id;
    }

    public async Task<bool> UpdateAsync(Order orderModel, CancellationToken cancellationToken)
    {
        var orderEntity = await GetByIdAsync(orderModel.Id, cancellationToken);

        if (orderEntity == null)
            return false;

        var entityPayments = orderEntity.Payments.ToDictionary(p => p.Id);

        orderEntity.Status = orderModel.Status;

        foreach (var payment in orderModel.Payments)
        {
            if (entityPayments.TryGetValue(payment.Id, out var paymentEntity))
                paymentEntity.Status = payment.Status;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<OrderEntity?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        return await _dbContext.Orders
            .Where(x => !x.IsDeleted)
            .Include(x => x.Payments)
            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);
    }
}
