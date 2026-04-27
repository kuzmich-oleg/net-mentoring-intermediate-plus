using Microsoft.EntityFrameworkCore;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Common.Extensions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Orders;

internal sealed class OrderReadRepository : IOrderReadRepository
{
    private readonly TicketingDbContext _dbContext;

    private IQueryable<OrderEntity> ActiveOrders
        => _dbContext.Orders.Where(x => !x.IsDeleted);

    public OrderReadRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var orderEntity = await ActiveOrders
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Cart)
            .Include(x => x.Payments.Where(p => !p.IsDeleted))
            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

        var orderModel = orderEntity.MapIfNotNull(OrderMapper.FromEntity);
        return orderModel;
    }

    public async Task<Order?> GetByCartIdAsync(Guid cartId, CancellationToken cancellationToken)
    {
        var orderEntity = await ActiveOrders
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Cart)
            .Include(x => x.Payments.Where(p => !p.IsDeleted))
            .FirstOrDefaultAsync(x => x.CartId == cartId, cancellationToken);

        var orderModel = orderEntity.MapIfNotNull(OrderMapper.FromEntity);
        return orderModel;
    }

    public async Task<Order?> GetByPaymentIdAsync(Guid paymentId, CancellationToken cancellationToken)
    {
        var orderEntity = await ActiveOrders
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Cart)
                .ThenInclude(x => x!.Items.Where(x => !x.IsDeleted))
                .ThenInclude(x => x.Offer)
            .Include(x => x.Payments.Where(p => !p.IsDeleted))
            .FirstOrDefaultAsync(x => x.Payments.Any(p => p.Id == paymentId), cancellationToken);

        var orderModel = orderEntity.MapIfNotNull(OrderMapper.FromEntity);
        return orderModel;
    }

    public async Task<IList<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var orderEntities = await ActiveOrders
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Cart)
            .Include(x => x.Payments.Where(p => !p.IsDeleted))
            .Where(x => x.CustomerId == customerId)
            .ToListAsync(cancellationToken);

        var orderModels = orderEntities.MapToArray(OrderMapper.FromEntity);
        return orderModels;
    }
}
