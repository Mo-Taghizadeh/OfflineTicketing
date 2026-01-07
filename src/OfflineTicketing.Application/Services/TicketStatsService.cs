using OfflineTicketing.Application.Abstractions;
using OfflineTicketing.Application.Contracts.Tickets;
using OfflineTicketing.Domain.Enums;

namespace OfflineTicketing.Application.Services;

public sealed class TicketStatsService
{
    private readonly ITicketRepository _tickets;

    public TicketStatsService(ITicketRepository tickets)
    {
        _tickets = tickets;
    }

    public async Task<TicketStatsResponse> GetAsync(CancellationToken ct)
    {
        var all = await _tickets.GetAllAsync(ct);
        var open = all.Count(x => x.Status == TicketStatus.Open);
        var inProg = all.Count(x => x.Status == TicketStatus.InProgress);
        var closed = all.Count(x => x.Status == TicketStatus.Closed);
        return new TicketStatsResponse(open, inProg, closed, all.Count);
    }
}
