using System.Globalization;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Mapster;
using ScheduleService.Application.Contracts.UserService.Group.Dto.Responses;
using ScheduleService.Application.Contracts.UserService.Teacher.dto.responses;
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
        ConfigureClassEntities();
        ConfigureUserServiceModels();
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
            Application.CQRS.ClassEntity.Queries.GetClasses.ClassDetailBase,
            ClassDetailBase
        >
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Subject, src => src.Subject)
            .Map(dest => dest.StartsAt, src => src.StartsAt.ToDuration())
            .Map(dest => dest.EndsAt, src => src.EndsAt.ToDuration())
            .Map(dest => dest.Rooms, src => src.Rooms);

        TypeAdapterConfig<
            Application.CQRS.ClassEntity.Queries.GetClasses.ClassDetailBase,
            ClassDetailBase
        >
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.ChangeOn, src => src.ChangeOn)
            .Map(dest => dest.Rooms, src => src.Rooms)
            .Map(dest => dest.Subject, src => src.Subject)
            .Map(dest => dest.StartsAt, src => src.StartsAt.ToDuration())
            .Map(dest => dest.EndsAt, src => src.EndsAt.ToDuration());
    }

    private static void ConfigureClassEntities()
    {
        TypeAdapterConfig<
            Application.CQRS.ClassEntity.Queries.GetClasses.Student.StudentClassDetailDto,
            StudentClassDetail
        >
            .NewConfig()
            .Map(dest => dest.StartsAt, src => src.StartsAt.ToDuration())
            .Map(dest => dest.EndsAt, src => src.EndsAt.ToDuration());

        TypeAdapterConfig<
            Application.CQRS.ClassEntity.Queries.GetClasses.Teacher.TeacherClassDetailDto,
            TeacherClassDetail
        >
            .NewConfig()
            .Map(dest => dest.StartsAt, src => src.StartsAt.ToDuration())
            .Map(dest => dest.EndsAt, src => src.EndsAt.ToDuration());

        TypeAdapterConfig<Domain.Entities.Class, Class>
            .NewConfig()
            .Map(dest => dest.StartsAt, src => src.StartsAt.ToDuration())
            .Map(dest => dest.EndsAt, src => src.EndsAt.ToDuration());
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

    private static void ConfigureUserServiceModels()
    {
        const string UserServiceTimeFormat = "MM/dd/yyyy HH:mm:ss";

        TypeAdapterConfig<UserServiceClient.GroupModel, GroupDto>
            .NewConfig()
            .Map(
                dest => dest.StartedAt,
                src =>
                    DateTime.ParseExact(
                        src.StartedAt,
                        UserServiceTimeFormat,
                        CultureInfo.InvariantCulture
                    )
            )
            .Map(
                dest => dest.GraduatedAt,
                src =>
                    src.GraduatedAt == null
                        ? (DateTime?)null
                        : DateTime.ParseExact(
                            src.GraduatedAt,
                            UserServiceTimeFormat,
                            CultureInfo.InvariantCulture
                        )
            );

        TypeAdapterConfig<UserServiceClient.TeacherModel, TeacherDto>
            .NewConfig()
            .Map(
                dest => dest.FiredAt,
                src =>
                    src.FiredAt == null
                        ? (DateTime?)null
                        : DateTime.ParseExact(
                            src.FiredAt,
                            UserServiceTimeFormat,
                            CultureInfo.InvariantCulture
                        )
            );
    }
}
