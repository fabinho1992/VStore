using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VStore.ProductApi.Domain.Models;

namespace VStore.ProductApi.Infrastructure.ProductContex;


public class DbProductContext : DbContext
{
    
    public DbProductContext(DbContextOptions<DbProductContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
