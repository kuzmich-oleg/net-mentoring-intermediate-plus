using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.DataAccess;

public class TicketingDbContextFactory : IDesignTimeDbContextFactory<TicketingDbContext>
{
    public TicketingDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TicketingDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=127.0.0.1,1433;Database=Ticketing;TrustServerCertificate=true;User Id=SA;Password=P@ssword11;",
            options => options
                .MigrationsHistoryTable(HistoryRepository.DefaultTableName, TicketingDbContext.SchemaName)
                .EnableRetryOnFailure());

        return new TicketingDbContext(optionsBuilder.Options);
    }
}
