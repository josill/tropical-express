using Microsoft.EntityFrameworkCore;
using TropicalExpress.Domain;

namespace TropicalExpress.Infrastructure;

public interface IAppDbContext
{
    public DbSet<Order> Orders { get; set; }
}