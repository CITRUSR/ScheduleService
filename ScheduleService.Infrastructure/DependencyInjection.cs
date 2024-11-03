using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScheduleService.Application.Contracts;

namespace ScheduleService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddSingleton<IDbContext, DbContext>();

        var connectionString = configuration.GetConnectionString("DbConnectionString");

        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        var upgrader = DeployChanges
            .To.PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(typeof(DependencyInjection).Assembly)
            .LogToConsole()
            .Build();

        upgrader.PerformUpgrade();

        return services;
    }
}
