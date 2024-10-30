using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ScheduleService.Infrastructure.Extensions;

public static class MigrationExtension
{
    public static IApplicationBuilder ApplyMigratrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var runner = scope.ServiceProvider.GetService<IMigrationRunner>();
        runner.ListMigrations();
        runner.MigrateUp();

        return app;
    }
}
