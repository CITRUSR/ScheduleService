using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ColorEntity.Commands.DeleteColor;

public record DeleteColorCommand(int Id) : IRequest<Color>;
