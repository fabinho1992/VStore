using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VStore.OrderApi.Domain.Models;

namespace VStore.OrderApi.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            // Apenas o relacionamento
            builder.HasMany(o => o.Items)
                  .WithOne(oi => oi.Order)
                  .HasForeignKey(oi => oi.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Configurações APENAS do Order
            builder.Property(o => o.TotalAmount).HasPrecision(18, 2);
            builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(50);

        }
    }
}
