using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ClassEntity.Commands.UpdateClass;

public record UpdateClassCommand(
    int ClassId,
    int GroupId,
    int SubjectId,
    int WeekdayId,
    int? ColorId,
    TimeSpan StartsAt,
    TimeSpan EndsAt,
    DateTime? ChangeOn,
    List<Guid> TeacherIds,
    List<int> RoomIds
) : IRequest<Class>;
