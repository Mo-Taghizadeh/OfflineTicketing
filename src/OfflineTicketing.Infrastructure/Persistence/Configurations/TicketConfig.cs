using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfflineTicketing.Domain.Entities;
using OfflineTicketing.Domain.Enums;

namespace OfflineTicketing.Infrastructure.Persistence.Configurations;

public sealed class TicketConfig : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> b)
    {
        b.ToTable("Tickets");
        b.HasKey(x => x.Id);

        b.Property(x => x.Title).HasMaxLength(200).IsRequired();
        b.Property(x => x.Description).IsRequired();

        b.Property(x => x.Status)
            .HasConversion(v => v.ToString(), v => Enum.Parse<TicketStatus>(v))
            .HasMaxLength(50)
            .IsRequired();

        b.Property(x => x.Priority)
            .HasConversion(v => v.ToString(), v => Enum.Parse<TicketPriority>(v))
            .HasMaxLength(50)
            .IsRequired();

        b.Property(x => x.CreatedAtUtc).IsRequired();
        b.Property(x => x.UpdatedAtUtc).IsRequired();

        b.HasIndex(x => x.CreatedByUserId);
        b.HasIndex(x => x.AssignedToUserId);
    }
}
