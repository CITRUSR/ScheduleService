using DbUp;
using Grpc.Core;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.UserService.Group;
using ScheduleService.Application.Contracts.UserService.Speciality;
using ScheduleService.Application.Contracts.UserService.Teacher;
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

        var userServiceUri = new Uri(configuration["Services:UserServiceUrl"]);
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

        AddGrpcClient<UserServiceClient.SpecialityService.SpecialityServiceClient>(
            services,
            userServiceUri,
            handler
        );

        AddGrpcClient<UserServiceClient.GroupService.GroupServiceClient>(
            services,
            userServiceUri,
            handler
        );

        AddGrpcClient<UserServiceClient.TeacherService.TeacherServiceClient>(
            services,
            userServiceUri,
            handler
        );

        services.AddSingleton<ISpecialityService, SpecialityService>();
        services.AddSingleton<ITeacherService, TeacherService>();
        services.AddSingleton<IGroupService, GroupService>();

        services.AddScoped<IColorRepository, ColorRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<IWeekdayRepository, WeekdayRepository>();
        services.AddScoped<ICurrentWeekdayRepository, CurrentWeekdayRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static void AddGrpcClient<T>(
        IServiceCollection services,
        Uri uri,
        HttpMessageHandler? handler
    )
        where T : ClientBase
    {
        services
            .AddGrpcClient<T>(options =>
            {
                options.Address = uri;
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return handler ?? new HttpClientHandler();
            });
    }
}
