using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Repositories.Carts;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.UnitTests.Repositories.Read;

public sealed class CartReadRepositoryTests : IDisposable
{
    private readonly TicketingDbContext _dbContext;
    private readonly CartReadRepository _repository;

    public CartReadRepositoryTests()
    {
        _dbContext = TicketingDbContextFactory.CreateInMemoryContext();
        _repository = new CartReadRepository(_dbContext);
    }

    public void Dispose()
    {
        _dbContext?.Dispose();
    }

    [Fact]
    public async Task GetByIdAsync_EntityDoesNotExist_ReturnsNull()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistingId, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_EntityExists_ReturnsValidModel()
    {
        // Arrange
        var existingId = Guid.NewGuid();
        var extectedStatus = CartStatus.Created;
        var fakeCart = new CartEntity
        {
            Id = existingId,
            Status = extectedStatus,
            Items = [
                new ()
                {
                    Id = Guid.NewGuid(),
                    Offer = new OfferEntity
                    {
                        Id = Guid.NewGuid(),
                        Price = 50.00m,
                        SeatPriceLevel = new () { Id = Guid.NewGuid() },
                        Seat = new () { Id = Guid.NewGuid() }
                    }
                }
            ]
        };

        _dbContext.Carts.Add(fakeCart);
        await _dbContext.SaveChangesAsync(default);

        // Act
        var result = await _repository.GetByIdAsync(existingId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeCart.Id, result.Id);
        Assert.Equal(extectedStatus, result.Status);
        Assert.NotNull(result.Items);
        Assert.NotEmpty(result.Items);

        Assert.All(result.Items, item =>
        {
            Assert.NotNull(item.Offer);
            Assert.NotNull(item.Offer.Seat);
            Assert.NotNull(item.Offer.SeatPriceLevel);
        });
    }

    [Fact]
    public async Task ExistsAsync_EntityDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _repository.ExistsAsync(nonExistingId, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ExistsAsync_EntityExists_ReturnsTrue()
    {
        // Arrange
        var existingId = Guid.NewGuid();
        var fakeCart = new CartEntity { Id = existingId };

        _dbContext.Carts.Add(fakeCart);
        await _dbContext.SaveChangesAsync(default);

        // Act
        var result = await _repository.ExistsAsync(existingId, CancellationToken.None);

        // Assert
        Assert.True(result);
    }
}
