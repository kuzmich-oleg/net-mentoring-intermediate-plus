using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Repositories.Orders;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.UnitTests.Repositories.Write;

public sealed class OrderWriteRepositoryTests : IDisposable
{
    private readonly TicketingDbContext _dbContext;
    private readonly OrderWriteRepository _repository;

    public OrderWriteRepositoryTests()
    {
        _dbContext = TicketingDbContextFactory.CreateInMemoryContext();
        _repository = new OrderWriteRepository(_dbContext);
    }

    public void Dispose()
    {
        _dbContext?.Dispose();
    }

    [Fact]
    public async Task AddAsync_ValidOrder_CreatesOrderInDb()
    {
        // Arrange
        var expectedCustomerId = Guid.NewGuid();
        var fakeOrder = new Order { CustomerId = expectedCustomerId };

        // Act
        var createdOrderId = await _repository.AddAsync(fakeOrder, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, createdOrderId);

        var createdOrderEntity = await _dbContext.Orders
            .FirstOrDefaultAsync(x => x.Id == createdOrderId, CancellationToken.None);

        Assert.NotNull(createdOrderEntity);
        Assert.Equal(expectedCustomerId, createdOrderEntity.CustomerId);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingOrder_ReturnsFalse()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();
        var fakeOrder = new Order { Id = nonExistingId };

        // Act
        var updateResult = await _repository.UpdateAsync(fakeOrder, CancellationToken.None);

        // Assert
        Assert.False(updateResult);
    }

    [Fact]
    public async Task UpdateAsync_ExistingOrder_ReturnsTrueAndUpdatesPaymentsStatus()
    {
        // Arrange
        var existingId = Guid.NewGuid();
        var existingPaymentId = Guid.NewGuid();
        var existingOrder = new OrderEntity {
            Id = existingId,
            Status = OrderStatus.Placed,
            Payments = [ new () { Id = existingPaymentId, Status = PaymentStatus.Pending } ]
        };

        _dbContext.Orders.Add(existingOrder);
        await _dbContext.SaveChangesAsync();

        var expectedPaymentStatus = PaymentStatus.Completed;
        var expectedOrderStatus = OrderStatus.Paid;
        var fakeOrder = new Order
        {
            Id = existingId, 
            Status = expectedOrderStatus,
            Payments = [ new () { Id = existingPaymentId, Status = expectedPaymentStatus }]
        };

        // Act
        var updateResult = await _repository.UpdateAsync(fakeOrder, CancellationToken.None);
        // Assert
        Assert.True(updateResult);

        var updatedOrderEntity = await _dbContext.Orders
            .FirstOrDefaultAsync(x => x.Id == existingId, CancellationToken.None);

        Assert.NotNull(updatedOrderEntity);
        Assert.Equal(expectedOrderStatus, updatedOrderEntity.Status);
        Assert.All(updatedOrderEntity.Payments, payment => Assert.Equal(expectedPaymentStatus, payment.Status));
    }
}
