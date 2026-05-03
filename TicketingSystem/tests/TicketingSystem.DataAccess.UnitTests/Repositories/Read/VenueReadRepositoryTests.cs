using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Repositories.Venues;

namespace TicketingSystem.DataAccess.UnitTests.Repositories.Read;

public sealed class VenueReadRepositoryTests : IDisposable
{
    private readonly TicketingDbContext _dbContext;
    private readonly VenueReadRepository _repository;

    public VenueReadRepositoryTests()
    {
        _dbContext = TicketingDbContextFactory.CreateInMemoryContext();
        _repository = new VenueReadRepository(_dbContext);
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
        var fakeVenue = new VenueEntity
        {
            Id = existingId,
            Name = "Test Venue",
            Location = "Test Location",
            Sections = [ 
                new SectionEntity() { Id = Guid.NewGuid(), Code = "Test Section", Rows = [
                        new SectionRowEntity() { Id = Guid.NewGuid(), Code = "Test Row", Seats = [
                               new SeatEntity() { Id = Guid.NewGuid(), SeatNumber = 123 } 
                            ] }
                    ] }
                ]
        };

        _dbContext.Venues.Add(fakeVenue);
        await _dbContext.SaveChangesAsync(default);

        // Act
        var result = await _repository.GetByIdAsync(existingId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeVenue.Id, result.Id);
        Assert.Equal(fakeVenue.Name, result.Name);
        Assert.Equal(fakeVenue.Location, result.Location);
        Assert.NotEmpty(result.Sections);
        Assert.All(result.Sections, section => Assert.NotEmpty(section.Rows));
        Assert.All(result.Sections, section 
            => Assert.All(section.Rows, row => Assert.NotEmpty(row.Seats)));
    }

    [Fact]
    public async Task GetVenuesAsync_EntitiesDoNotExist_ReturnsEmptyList()
    {
        // Act
        var result = await _repository.GetVenuesAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetVenuesAsync_EntitiesExist_ReturnsValidList()
    {
        // Arrange
        var fakeVenues = new List<VenueEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Venue 1", Location = "Location 1" },
            new() { Id = Guid.NewGuid(), Name = "Venue 2", Location = "Location 2" }
        };

        _dbContext.Venues.AddRange(fakeVenues);
        await _dbContext.SaveChangesAsync(default);

        // Act
        var result = await _repository.GetVenuesAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeVenues.Count, result.Count);
        Assert.All(result, item 
            => Assert.Contains(fakeVenues, fv => fv.Id == item.Id && fv.Name == item.Name && fv.Location == item.Location));
    }
}
