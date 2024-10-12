using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TropicalExpress.Domain;

namespace TropicalExpress.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(builder =>
        {
            builder.Property(o => o.Id)
                .HasConversion(
                    id => id.Value,
                    value => new OrderId(value)
                )
                .IsRequired();

            builder.OwnsMany(
                o => o.Fruits,
                b =>
                {
                    b.Property(f => f.FruitType).HasConversion<string>();
                    // b.Navigation(f => f.FruitPackaging).IsRequired(false);
                    // b.Navigation(f => f.TareWeight).IsRequired(false);

                    b.OwnsOne(f => f.NetWeight, nwb =>
                    {
                        nwb.OwnsOne(nw => nw.Weight, wb =>
                        {
                            wb.Property(w => w.Value).HasColumnName("NetWeight");
                            wb.Property(w => w.Unit).HasColumnName("NetWeightUnit").HasConversion<string>();
                        });
                    });

                    b.OwnsOne(f => f.TareWeight, twb =>
                    {
                        twb.OwnsOne(gw => gw.Weight, wb =>
                        {
                            wb.Property(w => w.Value).HasColumnName("TareWeight");
                            wb.Property(w => w.Unit).HasColumnName("TareWeightUnit").HasConversion<string>();
                        });
                    });
                    
                    b.OwnsOne(f => f.GrossWeight, gwb =>
                    {
                        gwb.OwnsOne(gw => gw.Weight, wb =>
                        {
                            wb.Property(w => w.Value).HasColumnName("GrossWeight");
                            wb.Property(w => w.Unit).HasColumnName("GrossWeightUnit").HasConversion<string>();
                        });
                    });

                    b.OwnsOne(f => f.FruitPackaging, fp =>
                    {
                        fp.OwnsOne(p => p.TareWeight, twb =>
                        {
                            twb.OwnsOne(tw => tw.Weight, wb =>
                            {
                                wb.Property(w => w.Value).HasColumnName("FruitPackagingTareWeight");
                                wb.Property(w => w.Unit).HasColumnName("FruitPackagingTareWeightUnit")
                                    .HasConversion<string>();
                            });
                        });
                    });
                });
        });
        
            base.OnModelCreating(modelBuilder);
        }
    
}