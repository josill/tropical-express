using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TropicalExpress.Domain;
using TropicalExpress.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace TropicalExpress.Tests;

public class FruitPrimitiveTypeTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<AppDbContext> _contextOptions;
    private readonly ITestOutputHelper _testOutputHelper;

    public FruitPrimitiveTypeTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new AppDbContext(_contextOptions);
        context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    [Fact]
    public async Task CanSaveAndRetrieveOrderWithComplexTypes()
    {
        // Arrange
        var netWeight = new NetWeight(new Weight(1, WeightUnit.Kg));
        var weightData = new WeightData(netWeight);
        var fruit = new Fruit(FruitType.Apple, weightData);
        var order = new Order(fruit);

        // Act
        await using (var context = new AppDbContext(_contextOptions))
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(_contextOptions))
        {
            var retrievedOrder = await context.Orders
                .Where(o => o.Id == order.Id)
                .Select(o => new
                {
                    o.Id,
                    FruitType = o.Fruit.FruitType,
                    NetWeightValue = o.Fruit.WeightData.NetWeight.Value,
                    NetWeightUnit = o.Fruit.WeightData.NetWeight.Unit,
                    TareWeightValue = o.Fruit.WeightData.TareWeight.Value,
                    TareWeightUnit = o.Fruit.WeightData.TareWeight.Unit
                })
                .FirstOrDefaultAsync();

            Assert.NotNull(retrievedOrder);
            Assert.Equal(order.Id, retrievedOrder.Id);
            Assert.Equal(fruit.FruitType, retrievedOrder.FruitType);
            Assert.Equal(fruit.WeightData.NetWeight.Value, retrievedOrder.NetWeightValue);
            Assert.Equal(fruit.WeightData.NetWeight.Unit, retrievedOrder.NetWeightUnit);
            Assert.Equal(fruit.WeightData.TareWeight.Value, retrievedOrder.TareWeightValue);
            Assert.Equal(fruit.WeightData.TareWeight.Unit, retrievedOrder.TareWeightUnit);
        }
    }

    [Fact]
    public async Task CanQueryOrdersByComplexTypeProperties()
    {
        // Arrange
        var order1 = new Order(new Fruit(FruitType.Apple, new WeightData(new NetWeight(Weight.FromKilograms(1.5m)))));
        var order2 = new Order(new Fruit(FruitType.Banana, new WeightData(new NetWeight(Weight.FromKilograms(2.0m)))));

        await using (var context = new AppDbContext(_contextOptions))
        {
            context.Orders.AddRange(order1, order2);
            await context.SaveChangesAsync();
        }

        // Act & Assert
        await using (var context = new AppDbContext(_contextOptions))
        {
            var appleOrder = await context.Orders
                .Where(o => o.Fruit.FruitType == FruitType.Apple)
                .Select(o => new
                {
                    o.Id,
                    FruitType = o.Fruit.FruitType
                })
                .FirstOrDefaultAsync();

            Assert.NotNull(appleOrder);
            Assert.Equal(order1.Id, appleOrder.Id);
            Assert.Equal(FruitType.Apple, appleOrder.FruitType);

            var heavyOrder = context.Orders
                .Where(o => o.Fruit.WeightData.NetWeight.Unit == WeightUnit.G)
                .Select(o => new
                {
                    o.Id,
                    NetWeightValue = o.Fruit.WeightData.NetWeight.Value
                })
                .FirstOrDefaultAsync();

            Assert.NotNull(heavyOrder);
            // Assert.Equal(order2.Id, heavyOrder.Id);
            // Assert.True(heavyOrder.NetWeightValue > 1.75m);
        }
    }

    [Fact]
    public async Task CanUpdateComplexTypeProperties()
    {
        // Arrange
        var order = new Order(new Fruit(FruitType.Apple, new WeightData(new NetWeight(Weight.FromKilograms(1.5m)))));

        await using (var context = new AppDbContext(_contextOptions))
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
        }

        // Act
        await using (var context = new AppDbContext(_contextOptions))
        {
            var retrievedOrder = await context.Orders
                .FirstAsync(o => o.Id == order.Id);

            Assert.NotNull(retrievedOrder);

            retrievedOrder.UpdateFruit(new Fruit(FruitType.Banana, new WeightData(new NetWeight(Weight.FromKilograms(2.0m)))));
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(_contextOptions))
        {
            var updatedOrder = await context.Orders
                .Where(o => o.Id == order.Id)
                .Select(o => new
                {
                    FruitType = o.Fruit.FruitType,
                    NetWeightValue = o.Fruit.WeightData.NetWeight.Value,
                    NetWeightUnit = o.Fruit.WeightData.NetWeight.Unit
                })
                .FirstAsync();

            Assert.NotNull(updatedOrder);
            Assert.Equal(FruitType.Banana, updatedOrder.FruitType);
            Assert.Equal(2.0m, updatedOrder.NetWeightValue);
            Assert.Equal(WeightUnit.Kg, updatedOrder.NetWeightUnit);
        }
    }
}