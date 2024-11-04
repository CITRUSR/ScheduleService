using MediatR;

namespace ScheduleService.Application.CQRS.ColorEntity.Commands.DeleteColor;

public record DeleteColorCommand(int Id) : IRequest<Unit>;
