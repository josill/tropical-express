using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TropicalExpress.Infrastructure;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        var connectionString = GetConnectionStringFromEnvironment();

        optionsBuilder.UseNpgsql(connectionString);

        var context = new AppDbContext(optionsBuilder.Options);
        context.Database.EnsureCreated();

        return context;
    }

    private static string GetConnectionStringFromEnvironment()
    {
        return Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ??
               "Server=localhost;Database=tropical-express;User Id=josill;Password=Strong@Password1";
    }
}