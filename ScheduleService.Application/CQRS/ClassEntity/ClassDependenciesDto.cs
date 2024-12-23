using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ClassEntity;

public record ClassDependenciesDto(
    Color? Color,
    Subject? Subject,
    Weekday? Weekday,
    List<Room> Rooms
);
