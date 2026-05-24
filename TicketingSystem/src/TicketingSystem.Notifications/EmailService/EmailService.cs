using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;

namespace TicketingSystem.Notifications.EmailService;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken);
}

internal sealed class EmailService : IEmailService
{
    private readonly IMailjetClient _mailjetClient;
    private readonly ILogger<EmailService> _logger;

    public EmailService(
        IMailjetClient mailjetClient,
        ILogger<EmailService> logger)
    {
        _mailjetClient = mailjetClient;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken)
    {
        var email = new TransactionalEmailBuilder()
            .WithFrom(new SendContact("from@test.com"))
            .WithSubject(subject)
            .WithHtmlPart(body)
            .WithTo(new SendContact(to))
            .Build();

        try
        {
            var response = await _mailjetClient.SendTransactionalEmailAsync(email, true);

            if (response.Messages.Length == 0)
            {
                _logger.LogError("Error: {StatusCode}", response.Messages.First().Errors);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while sending the email.");
        }
    }
}
