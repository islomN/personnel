using Database;
using Infrastructure.DataAccess;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .Configure<EntityContextOptions>(configuration.GetSection("DbSection"))
            .AddDbContext<EntityContext>()
            .AddScoped<IPersonnelService, PersonnelService>()
            .AddScoped<IEmployeeDataService, EmployeeDataService>()
            .AddScoped<IFileDataService, FileDataService>();
    }
}