namespace OfflineTicketing.Application.Contracts.Auth;

public sealed record LoginResponse(string AccessToken, DateTime ExpiresAtUtc);
