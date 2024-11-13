using ScheduleService.API.Interceptors;
using ScheduleService.Application;
using ScheduleService.Infrastructure;

namespace ScheduleService.API.extensions;

public static class SturtupExtension
{
    public static void ConfigureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<ServerExceptionsInterceptor>();
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddApplication();
        services.AddInfrastructure(configuration);

        services.AddGrpc();
    }

    public static void ConfigureApplication(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapGrpcService<Services.ColorService>();
        app.MapGrpcService<Services.RoomService>();
        app.MapGrpcService<Services.SubjectService>();
        app.MapGrpcService<Services.WeekdayService>();
        app.MapGrpcService<Services.CurrentWeekdayService>();
    }
}
