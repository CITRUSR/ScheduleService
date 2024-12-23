using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Mapster;
using ScheduleService.Application.CQRS.ClassEntity.Commands.CreateClass;
using ScheduleService.Application.CQRS.ClassEntity.Commands.UpdateClass;
using ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.CreateCurrentWeekday;
using ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.UpdateCurrentWeekday;

namespace ScheduleService.API;

public static class MapsterConfig
{
    public static void Configure()
    {
        TypeAdapterConfig.GlobalSettings.Default.UseDestinationValue(member =>
            member.SetterModifier == AccessModifier.None
            && member.Type.IsGenericType
            && member.Type.GetGenericTypeDefinition() == typeof(RepeatedField<>)
        );

        ConfigureCurrentWeekdayRequests();
        ConfigureCurrentWeekdayEntity();
        ConfigureClassRequests();
    }

    private static void ConfigureClassRequests()
    {
        TypeAdapterConfig<UpdateClassCommand, UpdateClassDto>
            .NewConfig()
            .Map(dest => dest.Id, src => src.ClassId);
    }

    private static void ConfigureCurrentWeekdayRequests()
    {
        TypeAdapterConfig<UpdateCurrentWeekdayRequest, UpdateCurrentWeekdayCommand>
            .NewConfig()
            .Map(dest => dest.Interval, src => src.Interval.ToTimeSpan())
            .Map(dest => dest.UpdateTime, src => src.UpdateTime.ToDateTime());

        TypeAdapterConfig<CreateCurrentWeekdayRequest, CreateCurrentWeekdayCommand>
            .NewConfig()
            .Map(dest => dest.Interval, src => src.Interval.ToTimeSpan())
            .Map(dest => dest.UpdateTime, src => src.UpdateTime.ToDateTime());
    }

    private static void ConfigureCurrentWeekdayEntity()
    {
        TypeAdapterConfig<Domain.Entities.CurrentWeekday, CurrentWeekday>
            .NewConfig()
            .Map(dest => dest.Interval, src => src.Interval.ToDuration());
    }
}
