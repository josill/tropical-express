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
    var netWeight = new Weight(1, WeightUnit.Kilograms);
    var netWeight2 = new Weight(1, WeightUnit.Kilograms);
    var net = new NetWeight(netWeight);
    var net2 = new NetWeight(netWeight2);
    var fruits = new List<Fruit>();
    var apple = new Fruit(FruitType.Apple, net);
    var banana = new Fruit(FruitType.Banana, net2);
    fruits.Add(apple);
    fruits.Add(banana);
    var order = new Order(fruits);

    dbContext.Orders.Add(order);
    await dbContext.SaveChangesAsync();
}


app.Run();