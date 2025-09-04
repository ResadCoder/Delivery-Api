using DeliveryAPi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryAPI.Persistence.Configurations;

public class OrderRequestConfiguration: IEntityTypeConfiguration<OrderRequest>
{
    public void Configure(EntityTypeBuilder<OrderRequest> builder)
    {
        builder
            .HasIndex(o => new{o.OrderId , o.CourierProfileId})
            .IsUnique();
    }
}