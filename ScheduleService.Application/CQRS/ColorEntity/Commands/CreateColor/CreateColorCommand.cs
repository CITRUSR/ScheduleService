using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ColorEntity.Commands.CreateColor;

public record CreateColorCommand(string Name) : IRequest<Color>;
