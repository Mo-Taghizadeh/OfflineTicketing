using OfflineTicketing.Domain.Enums;

namespace OfflineTicketing.Application.Abstractions;

public interface ICurrentUser
{
    Guid? UserId { get; }
    string? Email { get; }
    UserRole? Role { get; }
}
