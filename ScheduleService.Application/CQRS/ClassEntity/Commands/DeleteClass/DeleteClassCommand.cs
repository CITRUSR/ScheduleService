using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ClassEntity.Commands.DeleteClass;

public record DeleteClassCommand(int Id) : IRequest<Class>;
