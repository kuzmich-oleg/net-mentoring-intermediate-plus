using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Repositories.Payments;

namespace TicketingSystem.DataAccess.UnitTests.Repositories.Read;

public sealed class PaymentReadRepositoryTests : IDisposable
{
    private readonly TicketingDbContext _dbContext;
    private readonly PaymentReadRepository _repository;

    public PaymentReadRepositoryTests()
    {
        _dbContext = TicketingDbContextFactory.CreateInMemoryContext();
        _repository = new PaymentReadRepository(_dbContext);
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
        var fakePayment = new PaymentEntity
        {
            Id = existingId,
            Amount = 10m
        };

        _dbContext.Payments.Add(fakePayment);
        await _dbContext.SaveChangesAsync(default);

        // Act
        var result = await _repository.GetByIdAsync(existingId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakePayment.Id, result.Id);
        Assert.Equal(fakePayment.Amount, result.Amount);
    }
}
