using TicketingSystem.Common;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Repositories.Offers;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.UnitTests.Repositories.Read;

public sealed class OfferReadRepositoryTests : IDisposable
{
    private readonly TicketingDbContext _dbContext;
    private readonly OfferReadRepository _repository;

    public OfferReadRepositoryTests()
    {
        _dbContext = TicketingDbContextFactory.CreateInMemoryContext();
        _repository = new OfferReadRepository(_dbContext);
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
        var fakeOffer = new OfferEntity
        {
            Id = existingId,
            Price = 10m,
            SeatPriceLevel = new SeatPriceLevelEntity { Id = Guid.NewGuid(), PriceLevel = SeatPriceLevel.Adult },
            Seat = new SeatEntity
            {
                Id = Guid.NewGuid(),
                SeatNumber = 1,
                SectionRow = new SectionRowEntity 
                {
                    Id = Guid.NewGuid(),
                    Code = "Test Row Code",
                    Section = new SectionEntity
                    {
                        Id = Guid.NewGuid(),
                        Code = "Section A"
                    }
                }
            },
            Event = new EventEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test Event",
                Description = "Test Description",
                Venue = new VenueEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Venue",
                    Location = "Test Location"
                }
            }
        };

        _dbContext.Offers.Add(fakeOffer);
        await _dbContext.SaveChangesAsync(default);

        // Act
        var result = await _repository.GetByIdAsync(existingId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeOffer.Id, result.Id);
        Assert.Equal(fakeOffer.Price, result.Price);
        Assert.NotNull(result.SeatPriceLevel);
        Assert.NotNull(result.Seat);
        Assert.NotNull(result.Seat.SectionRow);
        Assert.NotNull(result.Seat.SectionRow.Section);
    }

    [Fact]
    public async Task GetEventOffersAsync_EntitiesDoNotExist_ReturnsEmptyList()
    {
        // Arange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _repository.GetEventOffersAsync(nonExistingId, null, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<Offer>>(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetEventOffersAsync_EntitiesExistAndNoFilters_ReturnsValidList()
    {
        // Arange
        var existingEventId = Guid.NewGuid();
        var fakeOffer = new OfferEntity
        {
            Id = Guid.NewGuid(),
            Price = 10m,
            SeatPriceLevel = new SeatPriceLevelEntity { Id = Guid.NewGuid(), PriceLevel = SeatPriceLevel.Adult },
            Seat = new SeatEntity
            {
                Id = Guid.NewGuid(),
                SeatNumber = 1,
                SectionRow = new SectionRowEntity
                {
                    Id = Guid.NewGuid(),
                    Code = "Test Row Code",
                    Section = new SectionEntity
                    {
                        Id = Guid.NewGuid(),
                        Code = "Section A"
                    }
                }
            },
            Event = new EventEntity
            {
                Id = existingEventId,
                Name = "Test Event",
                Description = "Test Description",
                Venue = new VenueEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Venue",
                    Location = "Test Location"
                }
            }
        };

        _dbContext.Offers.Add(fakeOffer);
        await _dbContext.SaveChangesAsync(default);

        // Act
        var result = await _repository.GetEventOffersAsync(existingEventId, null, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.All(result, offer =>
        {
            Assert.Equal(fakeOffer.Id, offer.Id);
            Assert.Equal(fakeOffer.Price, offer.Price);
            Assert.Equal(existingEventId, offer.EventId);
            Assert.NotNull(offer.Seat);
            Assert.NotNull(offer.Seat.SectionRow);
            Assert.NotNull(offer.Seat.SectionRow.Section);
        });
    }

    [Fact]
    public async Task GetEventOffersAsync_EntitiesExistAndSectionFilterApplied_ReturnsValidList()
    {
        // Arange
        var existingEventId = Guid.NewGuid();
        var existingSectionid = Guid.NewGuid();
        var fakeOffer = new OfferEntity
        {
            Id = Guid.NewGuid(),
            Price = 10m,
            SeatPriceLevel = new SeatPriceLevelEntity { Id = Guid.NewGuid(), PriceLevel = SeatPriceLevel.Adult },
            Seat = new SeatEntity
            {
                Id = Guid.NewGuid(),
                SeatNumber = 1,
                SectionRow = new SectionRowEntity
                {
                    Id = Guid.NewGuid(),
                    Code = "Test Row Code",
                    Section = new SectionEntity
                    {
                        Id = existingSectionid,
                        Code = "Section A"
                    }
                }
            },
            Event = new EventEntity
            {
                Id = existingEventId,
                Name = "Test Event",
                Description = "Test Description",
                Venue = new VenueEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Venue",
                    Location = "Test Location"
                }
            }
        };

        _dbContext.Offers.Add(fakeOffer);
        await _dbContext.SaveChangesAsync(default);

        // Act
        var result = await _repository.GetEventOffersAsync(existingEventId, existingSectionid, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.All(result, offer =>
        {
            Assert.Equal(fakeOffer.Id, offer.Id);
            Assert.Equal(fakeOffer.Price, offer.Price);
            Assert.Equal(existingEventId, offer.EventId);

            Assert.NotNull(offer.Seat);
            Assert.NotNull(offer.Seat.SectionRow);
            Assert.NotNull(offer.Seat.SectionRow.Section);

            Assert.Equal(existingSectionid, offer.Seat.SectionRow.Section.Id);
        });
    }
}
