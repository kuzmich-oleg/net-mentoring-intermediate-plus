using Alba;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;

namespace TicketingSystem.WebAPI.IntegrationTests;

public class WebAppFixture : IAsyncLifetime
{
    private const int DbExternalPortNumber = 1438;
    private const int ContainerDbPort = 1433;
    private const string EnvironmentName = "IntegrationTests";

    public WebAppFixture()
    {
    }

    public IAlbaHost Host { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        var sqlServerContainer = new ContainerBuilder("mcr.microsoft.com/mssql/server:2022-latest")
           .WithName($"sql-server-integration-tests-{Guid.NewGuid()}")
           .WithEnvironment("ACCEPT_EULA", "Y")
           .WithEnvironment("SA_PASSWORD", "IntegrationsP@ss1")
           .WithPortBinding(DbExternalPortNumber, ContainerDbPort)
           .WithWaitStrategy(Wait.ForUnixContainer()
               .UntilInternalTcpPortIsAvailable(ContainerDbPort))
           .Build();

        await sqlServerContainer.StartAsync();

        Host = await AlbaHost.For<Program>(builder =>
        {
            builder.UseEnvironment(EnvironmentName);
        });

        await Host.StartAsync();
    }

    public async Task DisposeAsync() => await Host.DisposeAsync();
}
