using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VStore.OrderApi.Domain.Models;

namespace VStore.OrderApi.Infrastructure.OrderContext.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.CustomerId).IsRequired();

            builder.Property(o => o.CreatedDate)
            .IsRequired();

            builder.Property(o => o.Status)
           .IsRequired()
           .HasConversion<string>();

            builder.Property(o => o.TotalAmount)
            .HasColumnType("decimal(18,2)");


            builder.HasMany(o => o.Items)
              .WithOne()
              .HasForeignKey("OrderId") 
              .OnDelete(DeleteBehavior.Cascade); 

        }
    }
}
