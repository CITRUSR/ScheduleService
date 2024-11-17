using Google.Protobuf.WellKnownTypes;
using Mapster;
using ScheduleService.API.extensions;
using ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.CreateCurrentWeekday;
using ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.UpdateCurrentWeekday;

namespace ScheduleService.API;

public static class MapsterConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<UpdateCurrentWeekdayRequest, UpdateCurrentWeekdayCommand>
            .NewConfig()
            .Map(dest => dest.Interval, src => src.Interval.ToTimeSpan())
            .Map(dest => dest.UpdateTime, src => src.UpdateTime.ToDateTime());
        TypeAdapterConfig<CreateCurrentWeekdayRequest, CreateCurrentWeekdayCommand>
            .NewConfig()
            .Map(dest => dest.Interval, src => src.Interval.ToTimeSpan())
            .Map(dest => dest.UpdateTime, src => src.UpdateTime.ToDateTime());
        TypeAdapterConfig<Domain.Entities.CurrentWeekday, CurrentWeekday>
            .NewConfig()
            .Map(dest => dest.Interval, src => src.Interval.ToDuration());
    }
}
