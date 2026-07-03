using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Trycore.Evm.Application.Interfaces;
using Trycore.Evm.Infrastructure.Persistence;
using Trycore.Evm.Infrastructure.Repositories;

namespace Trycore.Evm.Tests.Integration.Api;

public class EvmApiFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"EvmTestDb-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<EvmDbContext>>();
            services.RemoveAll<IProjectRepository>();

            services.AddDbContext<EvmDbContext>(options =>
            {
                options.UseInMemoryDatabase(_databaseName);
            });

            services.AddScoped<IProjectRepository, ProjectRepository>();

            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<EvmDbContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        });
    }
}