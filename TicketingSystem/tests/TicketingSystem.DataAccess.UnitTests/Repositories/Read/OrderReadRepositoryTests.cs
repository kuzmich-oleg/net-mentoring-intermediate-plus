using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Repositories.Orders;

namespace TicketingSystem.DataAccess.UnitTests.Repositories.Read;

public sealed class OrderReadRepositoryTests : IDisposable
{
    private readonly TicketingDbContext _dbContext;
    private readonly OrderReadRepository _repository;

    public OrderReadRepositoryTests()
    {
        _dbContext = TicketingDbContextFactory.CreateInMemoryContext();
        _repository = new OrderReadRepository(_dbContext);
    }

    public void Dispose()
    {
        _dbContext?.Dispose();
    }

    [Fact]
    public async Task GetByPaymentIdAsync_EntityDoesNotExist_ReturnsNull()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByPaymentIdAsync(nonExistingId, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByPaymentIdAsync_EntityExists_ReturnsValidModel()
    {
        // Arrange
        var existingPaymentId = Guid.NewGuid();
        var fakeOrder = new OrderEntity
        {
            Id = Guid.NewGuid(),
            Payments = [ new PaymentEntity { Id = existingPaymentId }],
            Cart = new CartEntity
            {
                Id = Guid.NewGuid(),
                Items = [
                    new CartItemEntity { Id = Guid.NewGuid(), Offer = new OfferEntity { Id = Guid.NewGuid() } },
                ]
            }
        };

        _dbContext.Orders.Add(fakeOrder);
        await _dbContext.SaveChangesAsync(default);

        // Act
        var result = await _repository.GetByPaymentIdAsync(existingPaymentId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Payments);
        Assert.NotEmpty(result.Payments);
        Assert.NotNull(result.Cart);
        Assert.NotNull(result.Cart.Items);
        Assert.NotEmpty(result.Cart.Items);
    }
}
