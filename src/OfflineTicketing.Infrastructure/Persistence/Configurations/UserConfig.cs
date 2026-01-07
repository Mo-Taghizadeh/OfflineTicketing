using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfflineTicketing.Domain.Entities;
using OfflineTicketing.Domain.Enums;

namespace OfflineTicketing.Infrastructure.Persistence.Configurations;

public sealed class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("Users");
        b.HasKey(x => x.Id);

        b.Property(x => x.FullName).HasMaxLength(200).IsRequired();
        b.Property(x => x.Email).HasMaxLength(256).IsRequired();
        b.HasIndex(x => x.Email).IsUnique();

        b.Property(x => x.PasswordHash).IsRequired();

        // store enums as string (readable)
        b.Property(x => x.Role)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<UserRole>(v))
            .HasMaxLength(50)
            .IsRequired();

        b.HasMany(x => x.CreatedTickets)
            .WithOne(x => x.CreatedByUser)
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasMany(x => x.AssignedTickets)
            .WithOne(x => x.AssignedToUser)
            .HasForeignKey(x => x.AssignedToUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
