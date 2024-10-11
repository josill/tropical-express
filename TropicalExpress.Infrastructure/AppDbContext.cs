using System.Text.Json;
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
            builder.Property(x => x.Id)
                .HasConversion(
                    id => id.Value,
                    value => new FruitId(value)
                )
                .IsRequired();
        });
        
        modelBuilder.Entity<Fruit>(entity =>
        {
            entity.Property(f => f.FruitWeightProfile)
                .HasConversion(
                    v => v.ToString(),
                    v => FruitWeightProfile.FromString(v)
                )
                .HasColumnName("WeightProfile");
        });
        
        base.OnModelCreating(modelBuilder);
    }
}