using Database.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Database;

public class EntityContext: BaseEntityContext
{
    public EntityContext(IOptions<EntityContextOptions> options, bool isUseLazyLoading)
    {
        ConnectionString = options.Value.ConnectionString;
        IsUseLazyLoading = isUseLazyLoading;
    }
    
    public EntityContext(IOptions<EntityContextOptions> options)
    {
        ConnectionString = options.Value.ConnectionString;
    }
    
    public EntityContext(string connectionString)
    {
        ConnectionString = connectionString;
    }
    
    public virtual DbSet<Employee> Employees { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            //.UseLazyLoadingProxies(IsUseLazyLoading)
            .UseNpgsql(
                ConnectionString,
                builder =>
                {
                    builder.EnableRetryOnFailure(
                        5,
                        TimeSpan.FromSeconds(2), 
                        null);
                });
    }
}