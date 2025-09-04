using DeliveryAPi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryAPI.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(u => u.Surname)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);
        builder.HasIndex(u => u.PhoneNumber).IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<int>();

        builder
            .HasOne(b => b.UserProfile)
            .WithOne(u => u.User)
            .HasForeignKey<UserProfile>(b => b.UserId);
        
        builder
            .HasOne(b => b.CurierProfile)
            .WithOne(u => u.User)
            .HasForeignKey<CurierProfile>(b => b.UserId);
    }
}