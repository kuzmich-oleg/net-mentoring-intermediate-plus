using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.SectionRows;

internal sealed class SectionRowWriteRepository : ISectionRowWriteRepository
{
    private readonly TicketingDbContext _dbContext;

    public SectionRowWriteRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(SectionRow sectionRowModel, CancellationToken cancellationToken)
    {
        var sectionRowEntity = SectionRowMapper.ToEntity(sectionRowModel);

        sectionRowEntity.Id = Guid.NewGuid();

        await _dbContext.SectionRows.AddAsync(sectionRowEntity, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return sectionRowEntity.Id;
    }

    public async Task<bool> UpdateAsync(SectionRow sectionRowModel, CancellationToken cancellationToken)
    {
        var sectionRowEntity = await GetByIdAsync(sectionRowModel.Id, cancellationToken);

        if (sectionRowEntity == null)
            return false;

        sectionRowEntity.Code = sectionRowModel.Code;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid sectionRowId, CancellationToken cancellationToken)
    {
        var sectionRowEntity = await GetByIdAsync(sectionRowId, cancellationToken);

        if (sectionRowEntity == null)
            return false;

        sectionRowEntity.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<SectionRowEntity?> GetByIdAsync(Guid sectionRowId, CancellationToken cancellationToken)
    {
        return await _dbContext.SectionRows
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == sectionRowId, cancellationToken);
    }
}
