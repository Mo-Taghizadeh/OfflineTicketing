using OfflineTicketing.Domain.Entities;

namespace OfflineTicketing.Application.Abstractions;

public interface ITicketRepository
{
    Task<Ticket?> FindByIdAsync(Guid id, CancellationToken ct);
    Task<List<Ticket>> GetAllAsync(CancellationToken ct);
    Task<List<Ticket>> GetByCreatorAsync(Guid creatorId, CancellationToken ct);
    Task AddAsync(Ticket ticket, CancellationToken ct);
    Task DeleteAsync(Ticket ticket, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
