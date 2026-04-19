using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Users;

internal sealed class UserWriteRepository : IUserWriteRepository
{
    private readonly TicketingDbContext _dbContext;

    public UserWriteRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(User userModel, CancellationToken cancellationToken)
    {
        var userEntity = UserMapper.ToEntity(userModel);

        userEntity.Id = Guid.NewGuid();

        await _dbContext.Users.AddAsync(userEntity, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return userEntity.Id;
    }

    public async Task<bool> UpdateAsync(User userModel, CancellationToken cancellationToken)
    {
        var userEntity = await GetByIdAsync(userModel.Id, cancellationToken);

        if (userEntity == null)
            return false;

        userEntity.Type = userModel.Type;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userEntity = await GetByIdAsync(userId, cancellationToken);

        if (userEntity == null)
            return false;

        userEntity.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<UserEntity?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }
}
