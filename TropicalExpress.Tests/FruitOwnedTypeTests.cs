using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TropicalExpress.Domain;
using TropicalExpress.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace TropicalExpress.Tests;

public class FruitOwnedTypeTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<AppDbContext> _contextOptions;
    private readonly ITestOutputHelper _testOutputHelper;

    public FruitOwnedTypeTests(ITestOutputHelper testOutputHelper)
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
    public async Task CannotSaveSameWeightObjectToMultipleFruits()
    {
        // Arrange
        var netWeight = new NetWeight(new Weight(1, WeightUnit.Kilograms));
        var fruit1 = new Fruit(FruitType.Apple, netWeight);
        var fruit2 = new Fruit(FruitType.Banana, netWeight);
        var order = new Order([fruit1, fruit2]);

        // Act & Assert
        await using var context = new AppDbContext(_contextOptions);
        context.Orders.Add(order);
        await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
    }

    [Fact]
    public async Task CanSaveAndRetrieveOrderWithComplexTypes()
    {
        // Arrange
        var netWeight1 = new NetWeight(new Weight(1, WeightUnit.Kilograms));
        var netWeight2 = new NetWeight(new Weight(1, WeightUnit.Kilograms));
        var fruit1 = new Fruit(FruitType.Apple, netWeight1);
        var fruit2 = new Fruit(FruitType.Banana, netWeight2);
        var order = new Order([fruit1, fruit2]);

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
                .Include(o => o.Fruits)
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            Assert.NotNull(retrievedOrder);
            Assert.Equal(order.Id, retrievedOrder.Id);
            Assert.Equal(2, retrievedOrder.Fruits.Count);
            
            var retrievedFruit1 = retrievedOrder.Fruits.First(f => f.FruitType == FruitType.Apple);
            var retrievedFruit2 = retrievedOrder.Fruits.First(f => f.FruitType == FruitType.Banana);
            
            Assert.Equal(fruit1.FruitType, retrievedFruit1.FruitType);
            Assert.Equal(fruit1.NetWeight.Weight.Value, retrievedFruit1.NetWeight.Weight.Value);
            Assert.Equal(fruit1.NetWeight.Weight.Unit, retrievedFruit1.NetWeight.Weight.Unit);
            
            Assert.Equal(fruit2.FruitType, retrievedFruit2.FruitType);
            Assert.Equal(fruit2.NetWeight.Weight.Value, retrievedFruit2.NetWeight.Weight.Value);
            Assert.Equal(fruit2.NetWeight.Weight.Unit, retrievedFruit2.NetWeight.Weight.Unit);
        }
    }

    [Fact]
    public async Task CanQueryOrdersByComplexTypeProperties()
    {
        // Arrange
        var order1 = new Order([new Fruit(FruitType.Apple, new NetWeight(Weight.FromKilograms(1.5m)))]);
        var order2 = new Order([new Fruit(FruitType.Banana, new NetWeight(Weight.FromKilograms(2.0m)))]);

        await using (var context = new AppDbContext(_contextOptions))
        {
            context.Orders.AddRange(order1, order2);
            await context.SaveChangesAsync();
        }

        // Act & Assert
        await using (var context = new AppDbContext(_contextOptions))
        {
            var appleOrder = await context.Orders
                .Include(o => o.Fruits)
                .FirstOrDefaultAsync(o => o.Fruits.Any(f => f.FruitType == FruitType.Apple));

            Assert.NotNull(appleOrder);
            Assert.Equal(order1.Id, appleOrder.Id);
            Assert.Contains(appleOrder.Fruits, f => f.FruitType == FruitType.Apple);

            var heavyOrder = await context.Orders
                .Include(o => o.Fruits)
                .FirstOrDefaultAsync(o => o.Fruits.Any(f => f.NetWeight.Weight.Value > 1.75m));

            Assert.NotNull(heavyOrder);
            Assert.Equal(order2.Id, heavyOrder.Id);
            Assert.Contains(heavyOrder.Fruits, f => f.NetWeight.Weight.Value > 1.75m);
        }
    }

    [Fact]
    public async Task CanUpdateComplexTypeProperties()
    {
        // Arrange
        var order = new Order(new List<Fruit> { new Fruit(FruitType.Apple, new NetWeight(Weight.FromKilograms(1.5m))) });

        await using (var context = new AppDbContext(_contextOptions))
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
        }

        // Act
        await using (var context = new AppDbContext(_contextOptions))
        {
            var retrievedOrder = await context.Orders
                .Include(o => o.Fruits)
                .FirstAsync(o => o.Id == order.Id);
        
            Assert.NotNull(retrievedOrder);

            retrievedOrder.RemoveFruit(retrievedOrder.Fruits.First());
            retrievedOrder.AddFruit(new Fruit(FruitType.Banana, new NetWeight(Weight.FromKilograms(2.0m))));
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(_contextOptions))
        {
            var updatedOrder = await context.Orders
                .Include(o => o.Fruits)
                .FirstAsync(o => o.Id == order.Id);
        
            Assert.NotNull(updatedOrder);
            Assert.Single(updatedOrder.Fruits);
            var updatedFruit = updatedOrder.Fruits.First();
            Assert.Equal(FruitType.Banana, updatedFruit.FruitType);
            Assert.Equal(2.0m, updatedFruit.NetWeight.Weight.Value);
            Assert.Equal(WeightUnit.Kilograms, updatedFruit.NetWeight.Weight.Unit);
        }
    }
}