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
    var tareWeight = new Weight(0.2m, WeightUnit.Kilograms);
    var net = new NetWeight(netWeight);
    var tare = new TareWeight(tareWeight);
    var package = new FruitPackaging(tare);
    var fruit1 = new Fruit(FruitType.Apple, net);
    var order = new Order(fruit1);

    dbContext.Orders.Add(order);
    await dbContext.SaveChangesAsync();
}


app.Run();