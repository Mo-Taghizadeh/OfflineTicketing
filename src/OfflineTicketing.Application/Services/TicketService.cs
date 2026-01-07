using OfflineTicketing.Application.Abstractions;
using OfflineTicketing.Application.Contracts.Tickets;
using OfflineTicketing.Domain.Entities;
using OfflineTicketing.Domain.Enums;

namespace OfflineTicketing.Application.Services;

public sealed class TicketService
{
    private readonly ITicketRepository _tickets;
    private readonly IUserRepository _users;

    public TicketService(ITicketRepository tickets, IUserRepository users)
    {
        _tickets = tickets;
        _users = users;
    }

    public async Task<TicketResponse> CreateAsync(Guid creatorUserId, CreateTicketRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ArgumentException("Title is required.");

        var now = DateTime.UtcNow;
        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            Description = request.Description?.Trim() ?? string.Empty,
            Priority = request.Priority,
            Status = TicketStatus.Open,
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            CreatedByUserId = creatorUserId,
            AssignedToUserId = null
        };

        await _tickets.AddAsync(ticket, ct);
        await _tickets.SaveChangesAsync(ct);

        return ToResponse(ticket);
    }

    public async Task<List<TicketResponse>> GetMyAsync(Guid creatorUserId, CancellationToken ct)
    {
        var items = await _tickets.GetByCreatorAsync(creatorUserId, ct);
        return items.Select(ToResponse).ToList();
    }

    public async Task<List<TicketResponse>> GetAllAsync(CancellationToken ct)
    {
        var items = await _tickets.GetAllAsync(ct);
        return items.Select(ToResponse).ToList();
    }

    public async Task<TicketResponse?> GetByIdForUserAsync(Guid ticketId, Guid currentUserId, UserRole role, CancellationToken ct)
    {
        var ticket = await _tickets.FindByIdAsync(ticketId, ct);
        if (ticket is null) return null;

        var allowed = role switch
        {
            UserRole.Employee => ticket.CreatedByUserId == currentUserId,
            UserRole.Admin => ticket.AssignedToUserId == currentUserId,
            _ => false
        };

        return allowed ? ToResponse(ticket) : null;
    }

    public async Task<TicketResponse?> UpdateByAdminAsync(Guid ticketId, UpdateTicketRequest request, CancellationToken ct)
    {
        var ticket = await _tickets.FindByIdAsync(ticketId, ct);
        if (ticket is null) return null;

        if (request.AssignedToUserId is not null)
        {
            var isAdmin = await _users.IsAdminAsync(request.AssignedToUserId.Value, ct);
            if (!isAdmin)
                throw new InvalidOperationException("AssignedToUserId must reference an Admin user.");
        }

        ticket.Status = request.Status;
        ticket.AssignedToUserId = request.AssignedToUserId;
        ticket.UpdatedAtUtc = DateTime.UtcNow;

        await _tickets.SaveChangesAsync(ct);
        return ToResponse(ticket);
    }

    public async Task<bool> DeleteAsync(Guid ticketId, CancellationToken ct)
    {
        var ticket = await _tickets.FindByIdAsync(ticketId, ct);
        if (ticket is null) return false;

        await _tickets.DeleteAsync(ticket, ct);
        await _tickets.SaveChangesAsync(ct);
        return true;
    }

    private static TicketResponse ToResponse(Ticket t)
        => new(t.Id, t.Title, t.Description, t.Status, t.Priority, t.CreatedAtUtc, t.UpdatedAtUtc, t.CreatedByUserId, t.AssignedToUserId);
}
