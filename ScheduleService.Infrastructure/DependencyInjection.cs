using System.Data;
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
        services.AddScoped(
            (sp) =>
            {
                var dbContext = sp.GetRequiredService<IDbContext>();
                return dbContext.CreateConnection();
            }
        );

        var connectionString = configuration.GetConnectionString("DbConnectionString");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("DbConnectionString is not configured.");
        }

        EnsureDatabase.For.PostgresqlDatabase(connectionString);
        PerformDatabaseUpgrade(connectionString);

        ConfigureHangfire(services, connectionString);
        services.AddSingleton<IScheduleService, Services.ScheduleService>();

        AddUserService(services, configuration);
        RegisterRepositories(services);

        return services;
    }

    private static void PerformDatabaseUpgrade(string connectionString)
    {
        var upgrader = DeployChanges
            .To.PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(typeof(DependencyInjection).Assembly)
            .LogToConsole()
            .Build();

        upgrader.PerformUpgrade();
    }

    private static void ConfigureHangfire(IServiceCollection services, string connectionString)
    {
        services.AddHangfireServer();
        services.AddHangfire(config =>
            config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(connectionString)
        );
    }

    private static void AddUserService(IServiceCollection services, IConfiguration configuration)
    {
        var userServiceUri = configuration["Services:UserServiceUrl"];

        if (string.IsNullOrEmpty(userServiceUri))
        {
            throw new InvalidOperationException("User ServiceUrl is not configured.");
        }

        var uri = new Uri(userServiceUri);
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

        AddGrpcClient<UserServiceClient.SpecialityService.SpecialityServiceClient>(
            services,
            uri,
            handler
        );

        AddGrpcClient<UserServiceClient.GroupService.GroupServiceClient>(services, uri, handler);

        AddGrpcClient<UserServiceClient.TeacherService.TeacherServiceClient>(
            services,
            uri,
            handler
        );

        services.AddSingleton<ISpecialityService, SpecialityService>();
        services.AddSingleton<ITeacherService, TeacherService>();
        services.AddSingleton<IGroupService, GroupService>();
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<IColorRepository, ColorRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<IWeekdayRepository, WeekdayRepository>();
        services.AddScoped<ICurrentWeekdayRepository, CurrentWeekdayRepository>();
        services.AddScoped<IClassRepository, ClassRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
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
