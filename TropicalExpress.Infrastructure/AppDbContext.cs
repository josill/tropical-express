using Microsoft.EntityFrameworkCore;
using TropicalExpress.Domain;
using TropicalExpress.Infrastructure.Configurations;

namespace TropicalExpress.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new OrderConfiguration().Configure(modelBuilder.Entity<Order>());
        
        base.OnModelCreating(modelBuilder);
    }
}