using TicketingSystem.Common.Configurations;

namespace TicketingSystem.Notifications.EmailService;

public sealed class EmailConfiguration : IConfig
{
    public static string SectionName => "Email";

    public required string ApiKey { get; init; }

    public required string ApiSecret { get; init; }
}
