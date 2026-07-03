using Microsoft.Extensions.DependencyInjection;
using Trycore.Evm.Application.Interfaces;
using Trycore.Evm.Application.Services;
using Trycore.Evm.Domain.Services;

namespace Trycore.Evm.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<EvmCalculator>();
        services.AddScoped<IProjectService, ProjectService>();

        return services;
    }
}