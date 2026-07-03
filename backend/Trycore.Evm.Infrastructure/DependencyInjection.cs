using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trycore.Evm.Application.Interfaces;
using Trycore.Evm.Infrastructure.Persistence;
using Trycore.Evm.Infrastructure.Repositories;

namespace Trycore.Evm.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<EvmDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IProjectRepository, ProjectRepository>();

        return services;
    }
}