using OfflineTicketing.Domain.Enums;

namespace OfflineTicketing.Application.Contracts.Tickets;

public sealed record TicketResponse(
    Guid Id,
    string Title,
    string Description,
    TicketStatus Status,
    TicketPriority Priority,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc,
    Guid CreatedByUserId,
    Guid? AssignedToUserId);
