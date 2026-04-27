namespace TicketingSystem.Application.Services.Orders.Models;

public sealed record DeleteCartItemCommand(Guid CartId, Guid EventId, Guid SeatId);
