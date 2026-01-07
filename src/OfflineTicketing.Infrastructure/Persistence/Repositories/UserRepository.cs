using Microsoft.EntityFrameworkCore;
using OfflineTicketing.Application.Abstractions;
using OfflineTicketing.Domain.Entities;
using OfflineTicketing.Domain.Enums;

namespace OfflineTicketing.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db) => _db = db;

    public Task<User?> FindByEmailAsync(string email, CancellationToken ct)
        => _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email, ct);

    public Task<User?> FindByIdAsync(Guid id, CancellationToken ct)
        => _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<bool> IsAdminAsync(Guid id, CancellationToken ct)
        => _db.Users.AsNoTracking().AnyAsync(x => x.Id == id && x.Role == UserRole.Admin, ct);

    public async Task AddAsync(User user, CancellationToken ct)
        => await _db.Users.AddAsync(user, ct);
}
