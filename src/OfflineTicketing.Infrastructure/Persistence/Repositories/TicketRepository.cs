using Microsoft.EntityFrameworkCore;
using OfflineTicketing.Application.Abstractions;
using OfflineTicketing.Domain.Entities;

namespace OfflineTicketing.Infrastructure.Persistence.Repositories;

public sealed class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _db;

    public TicketRepository(AppDbContext db) => _db = db;

    public Task<Ticket?> FindByIdAsync(Guid id, CancellationToken ct)
        => _db.Tickets.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<List<Ticket>> GetAllAsync(CancellationToken ct)
        => _db.Tickets.AsNoTracking().OrderByDescending(x => x.CreatedAtUtc).ToListAsync(ct);

    public Task<List<Ticket>> GetByCreatorAsync(Guid creatorId, CancellationToken ct)
        => _db.Tickets.AsNoTracking().Where(x => x.CreatedByUserId == creatorId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(ct);

    public async Task AddAsync(Ticket ticket, CancellationToken ct)
        => await _db.Tickets.AddAsync(ticket, ct);

    public Task DeleteAsync(Ticket ticket, CancellationToken ct)
    {
        _db.Tickets.Remove(ticket);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
}
