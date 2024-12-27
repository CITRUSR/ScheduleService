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
        ConfigureClassDtos();
    }

    private static void ConfigureClassDtos()
    {
        TypeAdapterConfig<
            IGrouping<Domain.Entities.Color?, Domain.Entities.Class>,
            Application.CQRS.ClassEntity.Queries.GetClasses.ColorClassesDto
        >
            .NewConfig()
            .Map(dest => dest.Color, src => src.Key)
            .Map(dest => dest.Classes, src => src);
    }

    private static void ConfigureClassRequests()
    {
        TypeAdapterConfig<CreateClassRequest, CreateClassCommand>
            .NewConfig()
            .Map(dest => dest.StartsAt, src => src.StartsAt.ToTimeSpan())
            .Map(dest => dest.EndsAt, src => src.EndsAt.ToTimeSpan())
            .Map(dest => dest.TeachersIds, src => src.TeacherIds);

        TypeAdapterConfig<UpdateClassRequest, UpdateClassCommand>
            .NewConfig()
            .Map(dest => dest.StartsAt, src => src.StartsAt.ToTimeSpan())
            .Map(dest => dest.EndsAt, src => src.EndsAt.ToTimeSpan())
            .Map(dest => dest.TeacherIds, src => src.TeacherIds);

        TypeAdapterConfig<UpdateClassCommand, UpdateClassDto>
            .NewConfig()
            .Map(dest => dest.Id, src => src.ClassId);

        TypeAdapterConfig<
            Application.CQRS.ClassEntity.Queries.GetClasses.ClassDetailDto,
            ClassDetailDto
        >
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Subject, src => src.Subject)
            .Map(dest => dest.StartsAt, src => src.StartsAt.ToDuration())
            .Map(dest => dest.EndsAt, src => src.EndsAt.ToDuration())
            .Map(
                dest => dest.TeacherIds,
                src => src.TeacherIds.Select(id => id.ToString()).ToList()
            )
            .Map(dest => dest.Rooms, src => src.Rooms);

        TypeAdapterConfig<
            Application.CQRS.ClassEntity.Queries.GetClasses.ColorClassesDto,
            ColorClassesDto
        >
            .NewConfig()
            .Map(dest => dest.Color, src => src.Color)
            .Map(dest => dest.Classes, src => src.Classes);

        TypeAdapterConfig<
            Application.CQRS.ClassEntity.Queries.GetClasses.GetClassOnCurrentDateForStudents.GetClassesOnCurrentDateForStudentResponse,
            GetClassesOnCurrentDateForStudentResponse
        >
            .NewConfig()
            .Map(dest => dest.GroupId, src => src.GroupId)
            .Map(dest => dest.Weekday, src => src.Weekday)
            .Map(dest => dest.Classes, src => src.Classes);
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
