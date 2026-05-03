using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Repositories.Offers;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.UnitTests.Repositories.Write;

public sealed class OfferWriteRepositoryTests : IDisposable
{
    private readonly TicketingDbContext _dbContext;
    private readonly OfferWriteRepository _repository;

    public OfferWriteRepositoryTests()
    {
        _dbContext = TicketingDbContextFactory.CreateInMemoryContext();
        _repository = new OfferWriteRepository(_dbContext);
    }

    public void Dispose()
    {
        _dbContext?.Dispose();
    }

    [Fact]
    public async Task AddAsync_ValidOffer_CreatesOfferInDb()
    {
        // Arrange
        var fakeOffer = new Offer { Price = 10m };

        // Act
        var createdOfferId = await _repository.AddAsync(fakeOffer, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, createdOfferId);

        var createdOfferEntity = await _dbContext.Offers    
            .FirstOrDefaultAsync(x => x.Id == createdOfferId, CancellationToken.None);

        Assert.NotNull(createdOfferEntity);
        Assert.Equal(fakeOffer.Price, createdOfferEntity.Price);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingOffer_ReturnsFalse()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();
        var fakeOffer = new Offer { Id = nonExistingId, Price = 10m };

        // Act
        var updateResult = await _repository.UpdateAsync(fakeOffer, CancellationToken.None);

        // Assert
        Assert.False(updateResult);
    }

    [Fact]
    public async Task UpdateAsync_ExistingOffer_ReturnsTrueAndUpdatesProps()
    {
        // Arrange
        var existingId = Guid.NewGuid();
        var existingOffeEntity = new OfferEntity { Id = existingId, Price = 5m, SeatStatus = SeatStatus.Available };
        
        _dbContext.Offers.Add(existingOffeEntity);
        await _dbContext.SaveChangesAsync();

        var expectedPrice = 10m;
        var expectedStatus = SeatStatus.Sold;
        var fakeOffer = new Offer { Id = existingId, Price = expectedPrice, SeatStatus = expectedStatus };

        // Act
        var updateResult = await _repository.UpdateAsync(fakeOffer, CancellationToken.None);

        // Assert
        Assert.True(updateResult);

        var updatedOfferEntity = await _dbContext.Offers
            .FirstOrDefaultAsync(x => x.Id == existingId, CancellationToken.None);

        Assert.NotNull(updatedOfferEntity);
        Assert.Equal(expectedPrice, updatedOfferEntity.Price);
        Assert.Equal(expectedStatus, updatedOfferEntity.SeatStatus);
    }

    [Fact]
    public async Task UpdateSeatStatusAsync_NullOfferIds_ReturnsFalse()
    {
        // Arrange
        Guid[] idsToUpdate = null!;

        // Act
        var nullResult = await _repository.UpdateSeatStatusAsync(idsToUpdate, SeatStatus.Booked, CancellationToken.None);
        
        // Assert
        Assert.False(nullResult);
    }

    [Fact]
    public async Task UpdateSeatStatusAsync_EmptyOfferIds_ReturnsFalse()
    {
        // Arrange
        var idsToUpdate = Array.Empty<Guid>();

        // Act
        var nullResult = await _repository.UpdateSeatStatusAsync(idsToUpdate, SeatStatus.Booked, CancellationToken.None);

        // Assert
        Assert.False(nullResult);
    }

    [Fact]
    public async Task UpdateSeatStatusAsync_ValidOfferIdsButLessEntitiesInDb_ReturnsFalse()
    {
        // Arrange
        var existingOffer = new OfferEntity { Id = Guid.NewGuid() };

        _dbContext.Offers.Add(existingOffer);
        await _dbContext.SaveChangesAsync();

        var idsToUpdate = new[] { existingOffer.Id, Guid.NewGuid() };

        // Act
        var updateResult = await _repository.UpdateSeatStatusAsync(idsToUpdate, SeatStatus.Booked, CancellationToken.None);
        
        // Assert
        Assert.False(updateResult);
    }

    [Fact]
    public async Task UpdateSeatStatusAsync_ValidOfferIds_ReturnsTrueAndUpdatesStatus()
    {
        // Arrange
        var venueId = Guid.NewGuid();
        var existingOffer1 = new OfferEntity
        {
            Id = Guid.NewGuid(),
            SeatStatus = SeatStatus.Available,
            Event = new EventEntity { Id = Guid.NewGuid(), Name = "Event 1", Description = "Description 1", Venue = new VenueEntity { Id = venueId, Name = "Venue 1", Location = "Location 1" } },
            Seat = new SeatEntity { Id = Guid.NewGuid(), SeatNumber = 1, SectionRow = new SectionRowEntity { Id = Guid.NewGuid(), Code = "Row 1", Section = new SectionEntity { Id = Guid.NewGuid(), VenueId = venueId, Code = "Section 1" } } },
            SeatPriceLevelId = Guid.Parse("bbbbbbbb-cccc-dddd-eeee-019dc8d2b0cb") //existing id from seed migration
        };
        var existingOffer2 = new OfferEntity
        {
            Id = Guid.NewGuid(),
            SeatStatus = SeatStatus.Available,
            Event = new EventEntity { Id = Guid.NewGuid(), Name = "Event 2", Description = "Description 2", Venue = new VenueEntity { Id = Guid.NewGuid(), Name = "Venue 2", Location = "Location 2" } },
            Seat = new SeatEntity { Id = Guid.NewGuid(), SeatNumber = 2, SectionRow = new SectionRowEntity { Id = Guid.NewGuid(), Code = "Row 2", Section = new SectionEntity { Id = Guid.NewGuid(), VenueId = venueId, Code = "Section 2" } } },
            SeatPriceLevelId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-019dc8d2b0ca") //existing id from seed migration
        };

        // InMemoryDbContext doesn't allow to use ExecuteUpdateAsync method.
        // So in this case will use SqlLite in-memory db
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<TicketingDbContext>()
            .UseSqlite(connection)
            .Options;
        using var context = new TicketingDbContext(options);
        var repository = new OfferWriteRepository(context);

        context.Database.EnsureCreated();
        context.Offers.AddRange(existingOffer1, existingOffer2);
        await context.SaveChangesAsync();

        var expectedStatus = SeatStatus.Sold;
        var idsToUpdate = new[] { existingOffer1.Id, existingOffer2.Id };

        // Act
        var updateResult = await repository.UpdateSeatStatusAsync(idsToUpdate, expectedStatus, CancellationToken.None);

        // Assert
        Assert.True(updateResult);
        context.ChangeTracker.Clear();

        var updatedOffers = await context.Offers
            .Where(x => idsToUpdate.Contains(x.Id))
            .ToListAsync(CancellationToken.None);

        Assert.All(updatedOffers, offer => Assert.Equal(expectedStatus, offer.SeatStatus));
    }

    [Fact]
    public async Task DeleteAsync_NonExistingOffer_ReturnsFalse()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var deleteResult = await _repository.DeleteAsync(nonExistingId, CancellationToken.None);

        // Assert
        Assert.False(deleteResult);
    }

    [Fact]
    public async Task DeleteAsync_ExistingCartItem_ReturnsTrueAndSetsIsDeleted()
    {
        // Arrange
        var existingOffer = new OfferEntity { Id = Guid.NewGuid() };

        _dbContext.Offers.Add(existingOffer);
        await _dbContext.SaveChangesAsync();

        // Act
        var deleteResult = await _repository.DeleteAsync(existingOffer.Id, CancellationToken.None);

        // Assert
        var deletedOffer = await _dbContext.Offers
            .FirstOrDefaultAsync(x => x.Id == existingOffer.Id, CancellationToken.None);

        Assert.NotNull(deletedOffer);
        Assert.True(deletedOffer.IsDeleted);
    }
}
