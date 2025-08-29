using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table
        builder.ToTable("Users");

        // Primary Key
        builder.HasKey(u => u.Id);

        builder.HasIndex(i => i.Email)
            .IsUnique();

        // Properties
        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Role)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        // Relationships
        builder.HasMany(u => u.Tickets)
            .WithOne(t => t.AssignedToUser)
            .HasForeignKey(t => t.AssignedToUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
