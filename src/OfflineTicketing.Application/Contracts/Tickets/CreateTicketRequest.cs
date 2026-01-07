using OfflineTicketing.Domain.Enums;

namespace OfflineTicketing.Application.Contracts.Tickets;

public sealed record CreateTicketRequest(string Title, string Description, TicketPriority Priority);
