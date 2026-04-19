using Microsoft.EntityFrameworkCore;
using TicketingSystem.Common.Extensions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Customers;

internal sealed class CustomerReadRepository : ICustomerReadRepository
{
    private readonly TicketingDbContext _dbContext;

    private IQueryable<CustomerEntity> ActiveCustomers
        => _dbContext.Customers.Where(x => !x.IsDeleted);

    public CustomerReadRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var customerEntity = await ActiveCustomers
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Tickets.Where(x => !x.IsDeleted))
                .ThenInclude(x => x.Offer)
                .ThenInclude(x => x!.Event)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        var customerModel = customerEntity.MapIfNotNull(CustomerMapper.FromEntity);
        return customerModel;
    }

    public async Task<Customer?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var customerEntity = await ActiveCustomers
            .AsNoTracking()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        var customerModel = customerEntity.MapIfNotNull(CustomerMapper.FromEntity);
        return customerModel;
    }
}
