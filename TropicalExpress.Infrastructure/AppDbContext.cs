using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TropicalExpress.Domain;

namespace TropicalExpress.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureOrderEntity(modelBuilder);
        
        base.OnModelCreating(modelBuilder);
    }
    
    /// <summary>
    /// Configures the Order entity.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    private void ConfigureOrderEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(builder =>
        {
            ConfigureOrderId(builder);
            ConfigureOrderComplexTypes(builder);
        });
    }

    /// <summary>
    /// Configures the Order ID property.
    /// </summary>
    /// <param name="builder">The entity type builder for the Order entity.</param>
    private void ConfigureOrderId(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.Id)
            .HasConversion(
                id => id.Value,
                value => new OrderId(value)
            )
            .IsRequired();
    }

    /// <summary>
    /// Configures the complex types for the Order entity.
    /// </summary>
    /// <param name="builder">The entity type builder for the Order entity.</param>
    private void ConfigureOrderComplexTypes(EntityTypeBuilder<Order> builder)
    {
        builder.ComplexProperty(o => o.Fruit, fruitBuilder =>
        {
            fruitBuilder.Property(f => f.FruitType).HasConversion<string>();
            fruitBuilder.ComplexProperty(f => f.NetWeight, netWeightBuilder =>
            {
                netWeightBuilder.ComplexProperty(nw => nw.Weight);
            });
        });
    }
}