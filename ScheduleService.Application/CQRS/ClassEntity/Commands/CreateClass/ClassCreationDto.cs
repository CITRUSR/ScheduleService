using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ClassEntity.Commands.CreateClass;

public record ClassCreationDto(Color? Color, Subject? Subject, Weekday? Weekday, List<Room> Rooms);
