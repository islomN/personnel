using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Database;

internal class EntityContextFactory : IDesignTimeDbContextFactory<EntityContext>
{
    public EntityContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("contextSettings.json", false, true)
            .Build();

        return new EntityContext(
            configuration.GetSection("DbSection:ConnectionString").Value!
        );
    }
}