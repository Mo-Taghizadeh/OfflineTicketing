using OfflineTicketing.Domain.Enums;

namespace OfflineTicketing.Application.Contracts.Tickets;

public sealed record UpdateTicketRequest(TicketStatus Status, Guid? AssignedToUserId);
