using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TropicalExpress.Domain;

namespace TropicalExpress.Infrastructure.Configurations;

public class OrderConfiguration:  IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> entityBuilder)
    {
        entityBuilder.Property(o => o.Id)
            .HasConversion(
                id => id.Value,
                value => new OrderId(value)
            )
            .IsRequired();
        
        new FruitConfiguration().Configure(entityBuilder.ComplexProperty(order => order.Fruit));
    }
}