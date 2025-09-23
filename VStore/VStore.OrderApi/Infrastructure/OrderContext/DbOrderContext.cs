using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VStore.OrderApi.Domain.Models;

namespace VStore.OrderApi.Infrastructure.OrderContext
{
    public class DbOrderContext : DbContext
    {
        public DbOrderContext(DbContextOptions<DbOrderContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

    }
}
