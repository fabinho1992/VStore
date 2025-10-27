using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VStore.OrderApi.Domain.Models;

namespace VStore.OrderApi.Infrastructure.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);
            builder.Property(oi => oi.Id).ValueGeneratedOnAdd();
            builder.Property(oi => oi.ProductId).IsRequired();
            builder.Property(oi => oi.UnitPrice).HasPrecision(18, 2);
            builder.Property(oi => oi.Subtotal).HasPrecision(18, 2);
            builder.Property(oi => oi.ProductName).HasMaxLength(200);

        }
    }
}
