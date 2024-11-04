using MediatR;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ColorEntity.Commands.UpdateColor;

public record UpdateColorCommand(int Id, string Name) : IRequest<Color>;
