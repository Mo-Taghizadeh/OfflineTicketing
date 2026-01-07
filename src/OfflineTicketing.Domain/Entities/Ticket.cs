using OfflineTicketing.Domain.Enums;

namespace OfflineTicketing.Domain.Entities;

public sealed class Ticket
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public TicketStatus Status { get; set; }
    public TicketPriority Priority { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public Guid CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = default!;

    public Guid? AssignedToUserId { get; set; }
    public User? AssignedToUser { get; set; }
}
