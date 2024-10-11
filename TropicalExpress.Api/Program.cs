using TropicalExpress.Infrastructure;
using Microsoft.AspNetCore.Builder;
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
    var fruitWeight = new Weight(1, Unit.Kilograms);
    var packageWeight = new Weight(200, Unit.Grams);
    var frp = new FruitWeightProfile(new NetWeight(fruitWeight), new TareWeight(packageWeight));
    var fruit = new Fruit(frp);
    dbContext.Fruits.Add(fruit);
    await dbContext.SaveChangesAsync();

    var retrievedFruit = dbContext.Fruits.FirstOrDefault();
    Console.WriteLine(retrievedFruit.FruitWeightProfile.NetWeight.Weight.Value);
    Console.WriteLine(retrievedFruit.FruitWeightProfile.NetWeight.Weight.Unit);
    Console.WriteLine(retrievedFruit.FruitWeightProfile.TareWeight.Weight.Unit);
    Console.WriteLine(retrievedFruit.FruitWeightProfile.TareWeight.Weight.Unit);
}


app.Run();