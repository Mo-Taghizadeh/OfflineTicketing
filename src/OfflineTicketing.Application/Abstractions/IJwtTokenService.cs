using OfflineTicketing.Domain.Entities;

namespace OfflineTicketing.Application.Abstractions;

public interface IJwtTokenService
{
    (string token, DateTime expiresAtUtc) CreateToken(User user);
}
