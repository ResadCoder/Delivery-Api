using DeliveryAPi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryAPI.Persistence.Configurations;

public class OrderConfiguration: IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.Description)
            .HasMaxLength(150); 

        
        builder.Property(o => o.DeliveryDate)
            .IsRequired();

        
        builder.Property(o => o.ImageUrl)
            .HasMaxLength(100); 

      
        builder.Property(o => o.Price)
            .IsRequired()
            .HasColumnType("decimal(6,2)");

        
        builder.Property(o => o.ReciverName)
            .IsRequired()
            .HasMaxLength(15);
        
        
        builder.Property(o => o.ReciverAddress)
            .IsRequired()
            .HasMaxLength(50);

   
        builder.Property(o => o.Rating)
            .HasColumnType("float"); 
        
        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<int>();
    }
}