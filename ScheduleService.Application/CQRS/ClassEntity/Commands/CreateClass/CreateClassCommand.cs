using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ClassEntity.Commands.CreateClass;

public record CreateClassCommand(
    int GroupId,
    int SubjectId,
    int WeekdayId,
    int? ColorId,
    TimeSpan StartsAt,
    TimeSpan EndsAt,
    DateTime? ChangeOn,
    List<Guid> TeachersIds,
    List<int> RoomIds
) : IRequest<Class>;
