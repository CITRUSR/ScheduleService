namespace ScheduleService.Application.CQRS.ClassEntity;

public record GetClassDependenciesDto(
    int GroupId,
    int SubjectId,
    int WeekdayId,
    int? ColorId,
    List<int> RoomIds
);
