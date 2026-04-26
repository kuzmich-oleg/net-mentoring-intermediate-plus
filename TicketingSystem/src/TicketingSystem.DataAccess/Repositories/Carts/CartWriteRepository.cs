using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Carts;

internal sealed class CartWriteRepository : ICartWriteRepository
{
    private readonly TicketingDbContext _dbContext;

    public CartWriteRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Guid> AddAsync(Cart cartModel, CancellationToken cancellationToken)
    {
        var cartEntity = CartMapper.ToEntity(cartModel);

        foreach (var cartItem in cartEntity.Items)
            cartItem.Id = Guid.NewGuid();

        _dbContext.Carts.Add(cartEntity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return cartEntity.Id;
    }
}
