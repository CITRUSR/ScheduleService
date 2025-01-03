namespace ScheduleService.Application.CQRS.ClassEntity.Commands.CreateClass;

public record CreateClassDto(
    int GroupId,
    int SubjectId,
    int WeekdayId,
    int? ColorId,
    TimeSpan StartsAt,
    TimeSpan EndsAt,
    DateTime? ChangeOn,
    List<Guid> TeacherIds,
    List<int> RoomIds
);
