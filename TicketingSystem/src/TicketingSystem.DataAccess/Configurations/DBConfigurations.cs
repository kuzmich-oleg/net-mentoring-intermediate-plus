using TicketingSystem.Common.Configurations;

namespace TicketingSystem.DataAccess.Configurations;

public class DBConfigurations : IConfig
{
    public static string SectionName => "DataBase";

    public bool ApplyMigrations { get; init; }
}
