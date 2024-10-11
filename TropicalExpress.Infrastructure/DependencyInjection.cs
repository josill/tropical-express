using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TropicalExpress.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration builderConfiguration
    )
    {
        services
            .AddPersistence(builderConfiguration);
            
        
        return services;
    }
    
    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = Environment.GetEnvironmentVariable("DefaultConnection")
                               ?? configuration["DefaultConnection"];

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString, o =>
            {
                o.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
            }));
        
        services.AddScoped<IAppDbContext, AppDbContext>();
        
        return services;
    }
}