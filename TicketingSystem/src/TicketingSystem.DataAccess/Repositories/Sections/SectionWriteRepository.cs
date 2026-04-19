using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Sections;

internal sealed class SectionWriteRepository : ISectionWriteRepository
{
    private readonly TicketingDbContext _dbContext;

    public SectionWriteRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(Section sectionModel, CancellationToken cancellationToken)
    {
        var sectionEntity = SectionMapper.ToEntity(sectionModel);

        sectionEntity.Id = Guid.NewGuid();

        await _dbContext.Sections.AddAsync(sectionEntity, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return sectionEntity.Id;
    }

    public async Task<bool> UpdateAsync(Section sectionModel, CancellationToken cancellationToken)
    {
        var sectionEntity = await GetByIdAsync(sectionModel.Id, cancellationToken);

        if (sectionEntity == null)
            return false;

        sectionEntity.Code = sectionModel.Code;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid sectionId, CancellationToken cancellationToken)
    {
        var sectionEntity = await GetByIdAsync(sectionId, cancellationToken);

        if (sectionEntity == null)
            return false;

        sectionEntity.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<SectionEntity?> GetByIdAsync(Guid sectionId, CancellationToken cancellationToken)
    {
        return await _dbContext.Sections
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == sectionId, cancellationToken);
    }
}
