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
    /// Configures the owned types for the Order entity.
    /// </summary>
    /// <param name="builder">The entity type builder for the Order entity.</param>
    public void ConfigureOrderComplexTypes(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsMany(o => o.Fruits, fruitBuilder =>
        {
            fruitBuilder.WithOwner().HasForeignKey("OrderId");
            fruitBuilder.Property<Guid>("Id");
            fruitBuilder.HasKey("Id");
            
            fruitBuilder.Property(f => f.FruitType).HasConversion<string>().HasColumnName("FruitType");

            fruitBuilder.OwnsOne(f => f.NetWeight, netWeightBuilder =>
            {
                netWeightBuilder.OwnsOne(p => p.Weight, weightBuilder =>
                {
                    weightBuilder.Property(w => w.Value).HasColumnName("NetWeightValue");
                    weightBuilder.Property(w => w.Unit).HasColumnName("NetWeightUnit");
                    weightBuilder.Property(w => w.Comment).HasColumnName("NetWeightComment");
                });
            });
        });
    }
}