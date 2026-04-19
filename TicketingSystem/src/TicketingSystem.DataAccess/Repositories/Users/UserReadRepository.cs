using Microsoft.EntityFrameworkCore;
using TicketingSystem.Common.Extensions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Users;

internal sealed class UserReadRepository : IUserReadRepository
{
    private readonly TicketingDbContext _dbContext;

    private IQueryable<UserEntity> ActiveUsers 
        => _dbContext.Users.Where(x => !x.IsDeleted);

    public UserReadRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var userEntity = await ActiveUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        var userModel = userEntity.MapIfNotNull(UserMapper.FromEntity);
        return userModel;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var userEntity = await ActiveUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        var userModel = userEntity.MapIfNotNull(UserMapper.FromEntity);
        return userModel;
    }
}
