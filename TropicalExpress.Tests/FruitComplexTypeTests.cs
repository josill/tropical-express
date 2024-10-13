using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TropicalExpress.Domain;
using TropicalExpress.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace TropicalExpress.Tests;

public class FruitComplexTypeTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<AppDbContext> _contextOptions;
    private readonly ITestOutputHelper _testOutputHelper;

    public FruitComplexTypeTests(ITestOutputHelper testOutputHelper)
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
        var netWeight = new Weight(1, WeightUnit.Kilograms);
        var net = new NetWeight(netWeight);
        var fruit = new Fruit(FruitType.Apple, net);
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
                // .Include(o => o.Fruit) would throw a run time error
                // with the message: The expression 'o.Fruit' is invalid inside an 'Include' operation, since it does not represent a property access
                .Select(o => new 
                {
                    o.Id,
                    o.Fruit.FruitType,
                    o.Fruit.NetWeight.Weight.Value,
                    o.Fruit.NetWeight.Weight.Unit
                }) // instead we have to map it to a new Order object
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            Assert.NotNull(retrievedOrder);
            Assert.Equal(order.Id, retrievedOrder.Id);
            Assert.Equal(order.Fruit.FruitType, retrievedOrder.FruitType);
            Assert.Equal(order.Fruit.NetWeight.Weight.Value, retrievedOrder.Value);
            Assert.Equal(order.Fruit.NetWeight.Weight.Unit, retrievedOrder.Unit);
        }
    }

    [Fact]
    public async Task CanQueryOrdersByComplexTypeProperties()
    {
        // Arrange
        var order1 = new Order(new Fruit(FruitType.Apple, new NetWeight(Weight.FromKilograms(1.5m))));
        var order2 = new Order(new Fruit(FruitType.Banana, new NetWeight(Weight.FromKilograms(2.0m))));

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
                .Select(o => new { o.Id, o.Fruit.FruitType })
                .FirstOrDefaultAsync();
            Assert.NotNull(appleOrder);
            Assert.Equal(order1.Id, appleOrder.Id);

            var heavyOrder = await context.Orders
                .Where(o => o.Fruit.NetWeight.Weight.Value > 1.75m)
                .Select(o => new { o.Id, Weight = o.Fruit.NetWeight.Weight.Value })
                .FirstOrDefaultAsync();
            Assert.NotNull(heavyOrder);
            Assert.Equal(order2.Id, heavyOrder.Id);
        }
    }

    [Fact]
    public async Task CanUpdateComplexTypeProperties()
    {
        // Arrange
        var order = new Order(new Fruit(FruitType.Apple, new NetWeight(Weight.FromKilograms(1.5m))));

        await using (var context = new AppDbContext(_contextOptions))
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
        }

        // Act
        await using (var context = new AppDbContext(_contextOptions))
        {
            var retrievedOrder = await context.Orders.FindAsync(order.Id);
        
            Assert.NotNull(retrievedOrder);

            retrievedOrder.UpdateFruit(new Fruit(FruitType.Banana, new NetWeight(Weight.FromKilograms(2.0m))));
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(_contextOptions))
        {
            var updatedOrder = await context.Orders
                .Where(o => o.Id == order.Id)
                .Select(o => new
                {
                    o.Id,
                    o.Fruit.FruitType,
                    Weight = o.Fruit.NetWeight.Weight.Value,
                    o.Fruit.NetWeight.Weight.Unit
                })
                .FirstOrDefaultAsync();
        
            Assert.NotNull(updatedOrder);
            Assert.Equal(FruitType.Banana, updatedOrder.FruitType);
            Assert.Equal(2.0m, updatedOrder.Weight);
            Assert.Equal(WeightUnit.Kilograms, updatedOrder.Unit);
        }
    }
}