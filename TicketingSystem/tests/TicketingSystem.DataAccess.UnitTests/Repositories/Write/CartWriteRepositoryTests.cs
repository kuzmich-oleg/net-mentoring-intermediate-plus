using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Repositories.Carts;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.UnitTests.Repositories.Write;

public sealed class CartWriteRepositoryTests : IDisposable
{
    private readonly TicketingDbContext _dbContext;
    private readonly CartWriteRepository _repository;

    public CartWriteRepositoryTests()
    {
        _dbContext = TicketingDbContextFactory.CreateInMemoryContext();
        _repository = new CartWriteRepository(_dbContext);
    }

    public void Dispose()
    {
        _dbContext?.Dispose();
    }

    [Fact]
    public async Task AddAsync_ValidCart_CreatesCartInDb()
    {
        // Arrange
        var fakeCart = new Cart { Id = Guid.NewGuid(), Items = [ new CartItem() ] };

        // Act
        var resultId = await _repository.AddAsync(fakeCart, CancellationToken.None);

        // Assert
        Assert.Equal(fakeCart.Id, resultId);

        var createdCartEntity = await _dbContext.Carts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == fakeCart.Id, CancellationToken.None);
        
        Assert.NotNull(createdCartEntity);
        Assert.Equal(fakeCart.Items.Count, createdCartEntity.Items.Count);
    }

    [Fact]
    public async Task AddCartItemAsync_ValidCartItem_CreatesCartItemInDb()
    {
        // Arrange
        var fakeCartId = Guid.NewGuid();
        var fakeCartItem = new CartItem { CartId = fakeCartId, OfferId = Guid.NewGuid() };
        var existingCart = new CartEntity { Id = fakeCartId, Items = [ new() { Id = Guid.NewGuid() }] };
        var expectedItemsCount = 2;

        _dbContext.Carts.Add(existingCart);
        await _dbContext.SaveChangesAsync();

        // Act
        var createdCartItemId = await _repository.AddCartItemAsync(fakeCartItem, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, createdCartItemId);
        
        var createdCartEntity = await _dbContext.Carts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == existingCart.Id, CancellationToken.None);

        Assert.NotNull(createdCartEntity);
        Assert.NotNull(createdCartEntity.Items);
        Assert.Equal(expectedItemsCount, createdCartEntity.Items.Count);
        Assert.Single(createdCartEntity.Items, x => x.OfferId == fakeCartItem.OfferId);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingCart_ReturnsFalse()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();
        var fakeCart = new Cart { Id = nonExistingId, Status = CartStatus.OrderPlaced };

        // Act
        var updateResult = await _repository.UpdateAsync(fakeCart, CancellationToken.None);

        // Assert
        Assert.False(updateResult);
    }

    [Fact]
    public async Task UpdateAsync_ExistingCart_ReturnsTrueAndUpdatesStatus()
    {
        // Arrange
        var existingId = Guid.NewGuid();
        var updatedStatus = CartStatus.OrderPlaced;
        var existingCart = new CartEntity { Id = existingId, Status = CartStatus.Created };
        var fakeCart = new Cart { Id = existingId, Status = updatedStatus };

        _dbContext.Carts.Add(existingCart);
        await _dbContext.SaveChangesAsync();

        // Act
        var updateResult = await _repository.UpdateAsync(fakeCart, CancellationToken.None);

        // Assert
        Assert.True(updateResult);

        var updatedCartEntity = await _dbContext.Carts
            .FirstOrDefaultAsync(x => x.Id == existingId, CancellationToken.None);

        Assert.NotNull(updatedCartEntity);
        Assert.Equal(updatedStatus, updatedCartEntity.Status);
    }

    [Fact]
    public async Task DeleteItemAsync_NonExistingCartItem_ReturnsFalse()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var deleteResult = await _repository.DeleteItemAsync(nonExistingId, CancellationToken.None);

        // Assert
        Assert.False(deleteResult);
    }

    [Fact]
    public async Task DeleteItemAsync_ExistingCartItem_ReturnsTrueAndSetsIsDeleted()
    {
        // Arrange
        var existingItemId = Guid.NewGuid();
        var existingCartItem = new CartItemEntity { Id = existingItemId, CartId = Guid.NewGuid(), OfferId = Guid.NewGuid() };
        var existingCart = new CartEntity { Id = existingCartItem.CartId, Items = [ existingCartItem ] };

        _dbContext.Carts.Add(existingCart);
        await _dbContext.SaveChangesAsync();

        // Act
        var deleteResult = await _repository.DeleteItemAsync(existingItemId, CancellationToken.None);

        // Assert
        Assert.True(deleteResult);

        var deletedCartItem = await _dbContext.CartItems
            .FirstOrDefaultAsync(x => x.Id == existingItemId, CancellationToken.None);

        Assert.NotNull(deletedCartItem);
        Assert.True(deletedCartItem.IsDeleted);
    }
}
