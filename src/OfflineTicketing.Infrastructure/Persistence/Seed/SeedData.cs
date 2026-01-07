using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using OfflineTicketing.Domain.Entities;
using OfflineTicketing.Domain.Enums;

namespace OfflineTicketing.Infrastructure.Persistence.Seed;

public static class SeedData
{
    public static async Task EnsureSeededAsync(AppDbContext db, IPasswordHasher<User> hasher, CancellationToken ct)
    {
        await db.Database.MigrateAsync(ct);

        if (!await db.Users.AnyAsync(ct))
        {
            var admin = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Local Admin",
                Email = "admin@local.com",
                Role = UserRole.Admin,
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");

            var employee = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Local Employee",
                Email = "employee@local.com",
                Role = UserRole.Employee,
            };
            employee.PasswordHash = hasher.HashPassword(employee, "Employee123!");

            db.Users.AddRange(admin, employee);

            db.Tickets.Add(new Ticket
            {
                Id = Guid.NewGuid(),
                Title = "Test Ticket",
                Description = "Testi",
                Status = TicketStatus.Open,
                Priority = TicketPriority.Medium,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow,
                CreatedByUserId = employee.Id,
                AssignedToUserId = admin.Id
            });

            await db.SaveChangesAsync(ct);
        }
    }
}
