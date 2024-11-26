using DbUp;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.UserService.Speciality;
using ScheduleService.Infrastructure.Repositories;
using ScheduleService.Infrastructure.Services.UserService;

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

        services.AddHangfireServer();
        services.AddHangfire(config =>
            config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(connectionString)
        );

        services.AddSingleton<IScheduleService, Services.ScheduleService>();

        services
            .AddGrpcClient<UserServiceClient.SpecialityService.SpecialityServiceClient>(options =>
            {
                options.Address = new Uri(configuration["Services:UserServiceUrl"]);
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                return handler;
            });

        services.AddSingleton<ISpecialityService, SpecialityService>();

        services.AddScoped<IColorRepository, ColorRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<IWeekdayRepository, WeekdayRepository>();
        services.AddScoped<ICurrentWeekdayRepository, CurrentWeekdayRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
