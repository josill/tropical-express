using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TropicalExpress.Domain;

namespace TropicalExpress.Infrastructure.Configurations;

public class FruitConfiguration
{
    public ComplexPropertyBuilder<Fruit> Configure(ComplexPropertyBuilder<Fruit> entityBuilder)
    {
        entityBuilder.Property(f => f.FruitType)
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .HasColumnName("FruitType");

        entityBuilder.Property<WeightData>(f => f.WeightData)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasConversion(
                weightData => WeightData.WeighDataToString(weightData),
                weightData => WeightData.StringToWeightData(weightData)
            )
            .HasColumnType("varchar(100)")
            .HasColumnName("WeightData");
        
        return entityBuilder;
    }
}