namespace OfflineTicketing.Application.Contracts.Tickets;

public sealed record TicketStatsResponse(int Open, int InProgress, int Closed, int Total);
