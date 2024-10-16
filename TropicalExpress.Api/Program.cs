using System.Text.Json;
using TropicalExpress.Infrastructure;
using Microsoft.EntityFrameworkCore;
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
    dbContext.Database.Migrate();
    dbContext.Database.EnsureCreated();

    // testing area
    var netWeight = new Weight(1, WeightUnit.Kg);
    var tareWeight = new Weight(200, WeightUnit.G);
    var net = new NetWeight(netWeight);
    var tare = new TareWeight(tareWeight);
    var weightData = new WeightData(net, tare);
    var fruit = new Fruit(FruitType.Banana, weightData);
    var order = new Order(fruit);

    dbContext.Orders.Add(order);
    await dbContext.SaveChangesAsync();

    var getOrder = await dbContext.Orders.FirstOrDefaultAsync();
    Console.WriteLine(getOrder.Fruit.FruitType);
    Console.WriteLine(getOrder.Fruit.WeightData.NetWeight);
    Console.WriteLine(getOrder.Fruit.WeightData.TareWeight);
    Console.WriteLine(getOrder.Fruit.WeightData.GrossWeight);
    Console.WriteLine(getOrder.Fruit.WeightData.NetWeight.Value);
    Console.WriteLine(getOrder.Fruit.WeightData.NetWeight.Unit);
    Console.WriteLine(getOrder.Fruit.WeightData.TareWeight.Value);
    Console.WriteLine(getOrder.Fruit.WeightData.TareWeight.Unit);
    if (getOrder != null)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
        };
        
        string jsonString = JsonSerializer.Serialize(getOrder, jsonOptions);
        Console.WriteLine("Order object as JSON:");
        Console.WriteLine(jsonString);
    }
    else
    {
        Console.WriteLine("No order found in the database.");
    }
}


app.Run();