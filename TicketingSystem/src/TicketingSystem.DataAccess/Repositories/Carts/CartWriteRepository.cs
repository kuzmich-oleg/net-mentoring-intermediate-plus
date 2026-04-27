using Microsoft.EntityFrameworkCore;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.DataAccess.Entities;
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

    public async Task<Guid> AddCartItemAsync(CartItem cartItemModel, CancellationToken cancellationToken)
    {
        var cartItemEntity = CartItemMapper.ToEntity(cartItemModel);

        cartItemEntity.Id = Guid.NewGuid();

        _dbContext.CartItems.Add(cartItemEntity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return cartItemEntity.Id;
    }

    public async Task<bool> UpdateAsync(Cart cartModel, CancellationToken cancellationToken)
    {
        var cartEntity = await GetByIdAsync(cartModel.Id, cancellationToken);

        if (cartEntity == null)
            return false;

        cartEntity.Status = cartModel.Status;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteItemAsync(Guid cartItemId, CancellationToken cancellationToken)
    {
        var cartItem = await GetCartItemByIdAsync(cartItemId, cancellationToken);

        if (cartItem == null)
            return false;

        cartItem.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<CartItemEntity?> GetCartItemByIdAsync(Guid cartItemId, CancellationToken cancellationToken)
    {
        var cartItem = await _dbContext.CartItems
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == cartItemId, cancellationToken);

        return cartItem;
    }

    private async Task<CartEntity?> GetByIdAsync(Guid cartId, CancellationToken cancellationToken)
    {
        var cartEntity = await _dbContext.Carts
            .FirstOrDefaultAsync(x => x.Id == cartId, cancellationToken);
        
        return cartEntity;
    }
}
