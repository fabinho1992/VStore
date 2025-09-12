using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VStore.ProductApi.Domain.Models;

namespace VStore.ProductApi.Infrastructure.ProductContex.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.HasMany(c => c.Products)
                   .WithOne(p => p.Catergory)
                   .HasForeignKey(p => p.CategoryId);
        }
    }
}
