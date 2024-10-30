using System.Reflection;
using FluentMigrator.Runner;
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

        services
            .AddLogging(c => c.AddFluentMigratorConsole())
            .AddFluentMigratorCore()
            .ConfigureRunner(c =>
                c.AddPostgres15_0()
                    .WithGlobalConnectionString(
                        configuration.GetConnectionString("DbConnectionString")
                    )
                    .ScanIn(typeof(DependencyInjection).Assembly)
                    .For.All()
            );

        return services;
    }
}
