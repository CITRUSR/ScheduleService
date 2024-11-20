using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ScheduleService.Application.Common.Behaviours;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Services;

namespace ScheduleService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        services.AddScoped<ICurrentWeekdayUpdateTaskCreator, CurrentWeekdayUpdateTaskCreator>();

        return services;
    }
}
