using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TropicalExpress.Infrastructure;

public class AppDbContextFactory: IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        var connectionString = GetConnectionStringFromEnvironment();
        
        optionsBuilder.UseSqlServer(connectionString);
        
        var context = new AppDbContext(optionsBuilder.Options);
        context.Database.EnsureCreated();
        
        return context;
    }
    
    private static string GetConnectionStringFromEnvironment()
    {
        return Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ?? "Server=sqlserver;Database=tropical-express;User Id=sa;Password=Strong@Password1;TrustServerCertificate=True;";
    }
}