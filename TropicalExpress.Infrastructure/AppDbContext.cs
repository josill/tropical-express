using Microsoft.EntityFrameworkCore;
using TropicalExpress.Domain;

namespace TropicalExpress.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<Fruit> Fruits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fruit>(builder =>
        {
            builder.Property(fruit => fruit.Id)
                .HasConversion(
                    id => id.Value,
                    value => new FruitId(value)
                )
                .IsRequired();
            
            builder.ComplexProperty(
                fruit => fruit.FruitWeightProfile,
                builder =>
                {
                    builder.ComplexProperty(fruit => fruit.NetWeight);
                    builder.ComplexProperty(fruit => fruit.TareWeight);
                    builder.ComplexProperty(fruit => fruit.GrossWeight);
                });
            {
                
            }
        });
        
        base.OnModelCreating(modelBuilder);
    }
}