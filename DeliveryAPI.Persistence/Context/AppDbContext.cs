using System.Reflection;
using DeliveryAPi.Domain.Entities;
using DeliveryAPi.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DeliveryAPI.Persistence.Context;

internal class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    
    public DbSet<Order> Orders { get; set; } = null!;
    
    public DbSet<CurierProfile> CurierProfiles { get; set; } = null!;
    
    public DbSet<UserProfile> UserProfiles { get; set; } = null!;
    
    public DbSet<OtpCode> OtpCodes { get; set; } = null!;
    
    public DbSet<UserAuthSession> UserAuthSessions { get; set; } = null!;
    public DbSet<OrderRequest> OrderRequests { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
    
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<EntityEntry<BaseEntity>> entries = ChangeTracker.Entries<BaseEntity>();

        foreach (EntityEntry<BaseEntity> entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}