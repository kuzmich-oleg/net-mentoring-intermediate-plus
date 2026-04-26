using Microsoft.EntityFrameworkCore;
using TicketingSystem.Common.Extensions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.SectionRows;

internal sealed class SectionRowReadRepository : ISectionRowReadRepository
{
    private readonly TicketingDbContext _dbContext;

    private IQueryable<SectionRowEntity> ActiveSectionRows
        => _dbContext.SectionRows.Where(x => !x.IsDeleted);

    public SectionRowReadRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SectionRow?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var sectionRowEntity = await ActiveSectionRows
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        var sectionRowModel = sectionRowEntity.MapIfNotNull(SectionRowMapper.FromEntity);
        return sectionRowModel;
    }
}
