using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Customers;

internal sealed class CustomerWriteRepository : ICustomerWriteRepository
{
    private readonly TicketingDbContext _dbContext;

    public CustomerWriteRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(Customer customerModel, CancellationToken cancellationToken)
    {
        var customerEntity = CustomerMapper.ToEntity(customerModel);

        customerEntity.Id = Guid.NewGuid();

        _dbContext.Customers.Add(customerEntity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return customerEntity.Id;
    }

    public async Task<bool> UpdateAsync(Customer customerModel, CancellationToken cancellationToken)
    {
        var customerEntity = await GetByIdAsync(customerModel.Id, cancellationToken);

        if (customerEntity == null)
            return false;

        customerEntity.FirstName = customerModel.FirstName;
        customerEntity.LastName = customerModel.LastName;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var customerEntity = await GetByIdAsync(customerId, cancellationToken);

        if (customerEntity == null)
            return false;

        customerEntity.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<CustomerEntity?> GetByIdAsync(Guid customerId, CancellationToken cancellationToken)
    {
        return await _dbContext.Customers
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == customerId, cancellationToken);
    }
}
