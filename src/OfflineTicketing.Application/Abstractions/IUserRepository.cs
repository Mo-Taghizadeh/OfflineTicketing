using OfflineTicketing.Domain.Entities;

namespace OfflineTicketing.Application.Abstractions;

public interface IUserRepository
{
    Task<User?> FindByEmailAsync(string email, CancellationToken ct);
    Task<User?> FindByIdAsync(Guid id, CancellationToken ct);
    Task<bool> IsAdminAsync(Guid id, CancellationToken ct);
    Task AddAsync(User user, CancellationToken ct);
}
