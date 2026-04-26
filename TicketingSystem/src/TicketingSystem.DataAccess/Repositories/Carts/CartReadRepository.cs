using Microsoft.EntityFrameworkCore;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Common.Extensions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Carts;

internal sealed class CartReadRepository : ICartReadRepository
{
    private readonly TicketingDbContext _dbContext;

    private IQueryable<CartEntity> ActiveCarts
        => _dbContext.Carts.Where(x => !x.IsDeleted);

    public CartReadRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var cartEntity = await ActiveCarts
            .Include(x => x.Items.Where(i => !i.IsDeleted))
                .ThenInclude(x => x.Offer)
                .ThenInclude(x => x!.Seat)
            .Include(x => x.Items.Where(i => !i.IsDeleted))
                .ThenInclude(x => x.Offer)
                .ThenInclude(x => x!.SeatPriceLevel)
            .Where(x => x.Status == CartStatus.Created)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        var model = cartEntity.MapIfNotNull(CartMapper.FromEntity);

        return model;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
       =>  await ActiveCarts.AnyAsync(x => x.Id == id, cancellationToken);
}
