using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScheduleService.Application.Contracts;
using ScheduleService.Infrastructure.Repositories;

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

        services.AddScoped<IColorRepository, ColorRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
