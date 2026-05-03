using TicketingSystem.Common;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Repositories.Events;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.UnitTests.Repositories.Read;

public sealed class EventReadRepositoryTests : IDisposable
{
    private readonly TicketingDbContext _dbContext;
    private readonly EventReadRepository _repository;

    public EventReadRepositoryTests()
    {
        _dbContext = TicketingDbContextFactory.CreateInMemoryContext();
        _repository = new EventReadRepository(_dbContext);
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
        var fakeEvent = new EventEntity {
            Id = existingId, 
            Name = "Test Event", 
            Description = "Test Description",
            Venue = new VenueEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test Venue",
                Location = "Test Location",
                Sections = [ new SectionEntity { Id = Guid.NewGuid(), Code = "Test Section"}] 
            }
        };

        _dbContext.Events.Add(fakeEvent);
        await _dbContext.SaveChangesAsync(default);

        // Act
        var result = await _repository.GetByIdAsync(existingId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEvent.Id, result.Id);
        Assert.Equal(fakeEvent.Name, result.Name);
        Assert.Equal(fakeEvent.Description, result.Description);

        Assert.NotNull(result.Venue);
        Assert.Equal(fakeEvent.Venue.Id, result.Venue.Id);
        Assert.Equal(fakeEvent.Venue.Name, result.Venue.Name);

        Assert.NotNull(result.Venue.Sections);
        Assert.All(result.Venue.Sections, section
            => Assert.Contains(fakeEvent.Venue.Sections, fs => fs.Id == section.Id && fs.Code == section.Code));
    }

    [Fact]
    public async Task GetEventsAsync_EntitiesDoNotExist_ReturnsEmptyList()
    {
        // Arange
        var fakeOffset = new OffsetPage(1, 10);

        // Act
        var result = await _repository.GetEventsAsync(null, null, fakeOffset, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<PagedResult<Event>>(result);
        Assert.Empty(result.Items);
        Assert.All(result.Items, @event => Assert.NotNull(@event.Venue));
    }

    [Fact]
    public async Task GetEventsAsync_EntitiesExistAndNoFilters_ReturnsValidList()
    {
        // Arrange
        var expectedCount = 2;
        var fakeOffset = new OffsetPage(1, 10);
        var fakeEvents = new List<EventEntity>
        {
            new () { Id = Guid.NewGuid(), Name = "Event 1", Description = "Description 1", Venue = new VenueEntity { Id = Guid.NewGuid(), Name = "Venue 1", Location = "Location 1" } },
            new () { Id = Guid.NewGuid(), Name = "Event 2", Description = "Description 2", Venue = new VenueEntity { Id = Guid.NewGuid(), Name = "Venue 2", Location = "Location 2" } }
        };

        _dbContext.Events.AddRange(fakeEvents);
        await _dbContext.SaveChangesAsync(default);

        // Act
        var result = await _repository.GetEventsAsync(null, null, fakeOffset, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<PagedResult<Event>>(result);
        Assert.Equal(expectedCount, result.Items.Count);
    }

    [Fact]
    public async Task GetEventsAsync_EntitiesExistAndDateFilterApplied_ReturnsValidList()
    {
        // Arrange
        var expectedCount = 1;
        var expectedDate = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var fakeOffset = new OffsetPage(1, 10);
        var fakeEvents = new List<EventEntity>  
        {
            new () { Id = Guid.NewGuid(), Name = "Event 1", EventDate = expectedDate, Description = "Description 1", Venue = new VenueEntity { Id = Guid.NewGuid(), Name = "Venue 1", Location = "Location 1" } },
            new () { Id = Guid.NewGuid(), Name = "Event 2", EventDate = expectedDate.AddDays(1), Description = "Description 2", Venue = new VenueEntity { Id = Guid.NewGuid(), Name = "Venue 2", Location = "Location 2" } }
        };

        _dbContext.Events.AddRange(fakeEvents);
        await _dbContext.SaveChangesAsync(default);

        // Act
        var result = await _repository.GetEventsAsync(null, expectedDate, fakeOffset, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<PagedResult<Event>>(result);
        Assert.Equal(expectedCount, result.Items.Count);
        Assert.All(result.Items, @event => Assert.NotNull(@event.Venue));
    }

    [Fact]
    public async Task GetEventsAsync_EntitiesExistAndNameFilterApplied_ReturnsValidList()
    {
        // Arrange
        var expectedCount = 1;
        var expectedNamePart = "1";
        var fakeOffset = new OffsetPage(1, 10);
        var fakeEvents = new List<EventEntity>
        {
            new () { Id = Guid.NewGuid(), Name = $"Event {expectedNamePart}", Description = "Description 1", Venue = new VenueEntity { Id = Guid.NewGuid(), Name = "Venue 1", Location = "Location 1" } },
            new () { Id = Guid.NewGuid(), Name = "Event 2", Description = "Description 2", Venue = new VenueEntity { Id = Guid.NewGuid(), Name = "Venue 2", Location = "Location 2" } }
        };

        _dbContext.Events.AddRange(fakeEvents);
        await _dbContext.SaveChangesAsync(default);

        // Act
        var result = await _repository.GetEventsAsync(expectedNamePart, null, fakeOffset, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<PagedResult<Event>>(result);
        Assert.Equal(expectedCount, result.Items.Count);
        Assert.All(result.Items, @event => Assert.NotNull(@event.Venue));
    }

    [Fact]
    public async Task GetEventsAsync_EntitiesExistAndAllFiltersApplied_ReturnsValidList()
    {
        // Arrange
        var expectedCount = 1;
        var expectedDate = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var expectedNamePart = "1";
        var fakeOffset = new OffsetPage(1, 10);
        var fakeEvents = new List<EventEntity>
        {
            new () { Id = Guid.NewGuid(), Name = $"Event {expectedNamePart}", EventDate = expectedDate, Description = "Description 1", Venue = new VenueEntity { Id = Guid.NewGuid(), Name = "Venue 1", Location = "Location 1" } },
            new () { Id = Guid.NewGuid(), Name = "Event 2", EventDate = expectedDate.AddDays(1), Description = "Description 2", Venue = new VenueEntity { Id = Guid.NewGuid(), Name = "Venue 2", Location = "Location 2" } }
        };

        _dbContext.Events.AddRange(fakeEvents);
        await _dbContext.SaveChangesAsync(default);

        // Act
        var result = await _repository.GetEventsAsync(expectedNamePart, expectedDate, fakeOffset, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<PagedResult<Event>>(result);
        Assert.Equal(expectedCount, result.Items.Count);
        Assert.All(result.Items, @event => Assert.NotNull(@event.Venue));
    }
}
