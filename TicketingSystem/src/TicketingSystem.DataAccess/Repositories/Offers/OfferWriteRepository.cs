using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Offers;

internal sealed class OfferWriteRepository : IOfferWriteRepository
{
    private readonly TicketingDbContext _dbContext;

    public OfferWriteRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(Offer offerModel, CancellationToken cancellationToken)
    {
        var offerEntity = OfferMapper.ToEntity(offerModel);

        offerEntity.Id = Guid.NewGuid();

        _dbContext.Offers.Add(offerEntity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return offerEntity.Id;
    }

    public async Task<bool> UpdateAsync(Offer offerModel, CancellationToken cancellationToken)
    {
        var offerEntity = await GetByIdAsync(offerModel.Id, cancellationToken);

        if (offerEntity == null)
            return false;

        offerEntity.Price = offerModel.Price;
        offerEntity.SeatStatus = offerModel.SeatStatus;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> UpdateSeatStatusAsync(Guid[] offerIds, SeatStatus expectedCurrentStatus,
        SeatStatus seatStatus, CancellationToken cancellationToken)
    {
        if (offerIds == null || offerIds.Length == 0)
            return false;

        var strategy = _dbContext.Database.CreateExecutionStrategy();

        var executionResult = await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(
                IsolationLevel.RepeatableRead, cancellationToken);

            try
            {
                var parameterNames = new string[offerIds.Length];
                var idsParams = new SqlParameter[offerIds.Length + 1];

                for (var i = 0; i < offerIds.Length; i++)
                {
                    parameterNames[i] = $"@p{i}";
                    idsParams[i] = new SqlParameter(parameterNames[i], offerIds[i]);
                }

                idsParams[offerIds.Length] = new SqlParameter("@expectedStatus", (int)expectedCurrentStatus);

                var offerEntities = await _dbContext.Offers
                    .FromSqlRaw(
                        $"SELECT * FROM [Ticketing].[Offers] WITH (UPDLOCK) WHERE Id IN ({string.Join(", ", parameterNames)}) AND IsDeleted = 0 AND SeatStatus = @expectedStatus",
                        idsParams)
                    .AsTracking()
                    .ToListAsync(cancellationToken);

                if (offerEntities.Count != offerIds.Length)
                    return false;

                foreach (var offerEntity in offerEntities)
                    offerEntity.SeatStatus = seatStatus;

                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return true;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                return false;
            }
        });

        return executionResult;
    }

    public async Task<bool> DeleteAsync(Guid offerId, CancellationToken cancellationToken)
    {
        var offerEntity = await GetByIdAsync(offerId, cancellationToken);

        if (offerEntity == null)
            return false;

        offerEntity.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<OfferEntity?> GetByIdAsync(Guid offerId, CancellationToken cancellationToken)
    {
        return await _dbContext.Offers
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == offerId, cancellationToken);
    }
}
