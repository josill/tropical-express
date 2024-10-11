using TropicalExpress.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TropicalExpress.Domain;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();

    // testing area
    Console.WriteLine("\nTesting Database Operations:");

    // Create and save a new Fruit
    var netWeight = new Weight(10.5m, WeightUnit.Kilograms);
    var tareWeight = new Weight(0.5m, WeightUnit.Kilograms);
    var fruitWeightProfile = new FruitWeightProfile(new NetWeight(netWeight), new TareWeight(tareWeight));
    var fruit = new Fruit(fruitWeightProfile);

    dbContext.Fruits.Add(fruit);
    await dbContext.SaveChangesAsync();

    Console.WriteLine("Fruit saved to database.");

    // Retrieve the Fruit
    var retrievedFruit = await dbContext.Fruits.FirstOrDefaultAsync();
    if (retrievedFruit != null)
    {
        Console.WriteLine("Retrieved Fruit from database:");
        Console.WriteLine($"Weight Profile: {retrievedFruit.FruitWeightProfile}");
        Console.WriteLine($"Net Weight: {retrievedFruit.FruitWeightProfile.NetWeight}");
        Console.WriteLine($"Tare Weight: {retrievedFruit.FruitWeightProfile.TareWeight}");
        Console.WriteLine($"Gross Weight: {retrievedFruit.FruitWeightProfile.GrossWeight}");
    }
    else
    {
        Console.WriteLine("No Fruit found in database.");
    }
}


app.Run();