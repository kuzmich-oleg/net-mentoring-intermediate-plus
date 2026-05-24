using TicketingSystem.Common.Events;

namespace TicketingSystem.Application.Interfaces.Services;

public interface INotificationService
{
    Task PublishEventAsync<T>(T @event, EventType eventType, CancellationToken cancellationToken)
        where T : BaseEvent;
}
