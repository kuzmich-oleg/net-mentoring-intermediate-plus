using Microsoft.EntityFrameworkCore;
using TicketingSystem.Common.Extensions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Sections;

internal sealed class SectionReadRepository : ISectionReadRepository
{
    private readonly TicketingDbContext _dbContext;

    private IQueryable<SectionEntity> ActiveSections
        => _dbContext.Sections.Where(x => !x.IsDeleted);

    public SectionReadRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Section?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var sectionEntity = await ActiveSections
            .AsNoTracking()
            .Include(x => x.Rows.Where(x => !x.IsDeleted))
            .ThenInclude(x => x.Seats.Where(x => !x.IsDeleted))
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        var sectionModel = sectionEntity.MapIfNotNull(SectionMapper.FromEntity);
        return sectionModel;
    }
}
